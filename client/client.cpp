#include <stdio.h>
#include <winsock2.h>
#include <iostream>
#pragma comment(lib, "ws2_32.lib")

#pragma warning(disable: 4996)

SOCKET Connection;

void ClientHandler() {
	char message[256];
	while (true) {
		recv(Connection, message, sizeof(message), NULL);
		std::cout << message << std::endl;
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
	address.sin_addr.s_addr = inet_addr("127.0.0.1"); //ip фдрес, указан localhost
	address.sin_port = htons(1111); //Порт для идентификации программы
	address.sin_family = AF_INET; //Семейство интернет протоколов

	Connection= socket(AF_INET, SOCK_STREAM, NULL);

	if (connect(Connection, (SOCKADDR*)&address, sizeof(address)) != 0) {
		std::cout << "Error: failed connect to server.\n";
		return 1;
	}
	std::cout << "Connected!\n";
	CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ClientHandler, NULL, NULL, NULL);

	char message[256];
	while (true) {
		std::cin.getline(message, sizeof(message));
		send(Connection, message, sizeof(message), NULL);
		Sleep(10);
	}

	system("pause");
	return 0;

}
