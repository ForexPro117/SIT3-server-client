#include <stdio.h>
#include <winsock2.h>
#include <iostream>
#include <thread>
#pragma comment(lib, "ws2_32.lib")

#pragma warning(disable: 4996)

const int size = 100;
SOCKET Connections[size];
int indexCounter = 0;

void ClientHandler(int index, std::string ip) {

	int msg_size;
	while (true) {
		if (recv(Connections[index], (char*)&msg_size, sizeof(int), NULL) > 0) {
			char* msg = new char[msg_size + 1];
			msg[msg_size] = '\0';
			recv(Connections[index], msg, msg_size, NULL);
			for (int i = 0; i < indexCounter; i++) {
				if (i == index || Connections[i] == INVALID_SOCKET) {
					continue;
				}
				send(Connections[i], (char*)&msg_size, sizeof(int), NULL);
				send(Connections[i], msg, msg_size, NULL);
			}
			delete[] msg;
		}
		else {
			::closesocket(Connections[index]);
			Connections[index] = INVALID_SOCKET;
			std::cout << "Client disconnected:" << ip << std::endl;
			return;
		}
	}
}


int main()
{
	WSAData wsaData;
	WORD DLLVersion = MAKEWORD(2, 1);
	if (WSAStartup(DLLVersion, &wsaData) != 0) {
		std::cout << "ERROR" << std::endl;
		exit(1);
	}

	SOCKADDR_IN address; //Структура для хранения адресов интернет протоколов
	int sizeOfAddress = sizeof(address);
	//добавить ввод ip
	std::cout << "Enter ip address:";
	char ip[20];
	std::cin >> ip;
	if (strchr(ip,'.') == 0)
		strcpy(ip, "127.0.0.1");
	address.sin_addr.s_addr = inet_addr(ip); //ip фдрес, указан localhost
	address.sin_port = htons(1111); //Порт для идентификации программы
	address.sin_family = AF_INET; //Семейство интернет протоколов

	SOCKET serverListener = socket(AF_INET, SOCK_STREAM, NULL); //Сокет для прослушивания входящих соединений

	bind(serverListener, (SOCKADDR*)&address, sizeof(address)); //Привязка сокету адреса
	std::cout << "Server started:" << inet_ntoa(address.sin_addr) << std::endl;
	listen(serverListener, SOMAXCONN); //Ожидание соединения с клиентом

	SOCKET	newConnection;

	std::thread threads[size];
	for (size_t i = 0; i < size; i++)
	{
		newConnection = accept(serverListener, (SOCKADDR*)&address, &sizeOfAddress); //Сокет для удержания соединения с клиентом
		if (newConnection == 0) //Проверка соединения
		{
			std::cout << "Error, no connection:" << inet_ntoa(address.sin_addr) << std::endl;
		}
		else {
			std::cout << "Client connected:" << inet_ntoa(address.sin_addr) << std::endl;
			std::string message = "You can send any messages!";
			int msg_size = message.size();
			send(newConnection, (char*)&msg_size, sizeof(int), NULL);
			send(newConnection, message.c_str(), message.size(), NULL); //Отправка сообщения клиентам

			Connections[i] = newConnection;
			indexCounter++;
			//CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ClientHandler,
				//(LPVOID)(i, inet_ntoa(address.sin_addr)), NULL, NULL);
			threads[i] = std::thread(ClientHandler, i, inet_ntoa(address.sin_addr));
		}
	}
	system("pause");

}

