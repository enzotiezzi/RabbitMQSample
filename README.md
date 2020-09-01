# RabbitMQSample

An example of how to do a simple communication using rabbitMQ and c#

To install rabbitMQ server on you environment, follow the steps from the installation guide

https://www.rabbitmq.com/download.html

After installing RabbitMQ server, open VS Solution, compile it(debug or release), then open the bin folder and execute the "LetsTalkToCBYK" application.

After executing "LetsTalkToCBYK" application, an instance will be ready to use, you can name it as you wish or use the default name just by hitting "enter".

The instance will start to send messages and listen on "localhost" and the default port "5672", make sure it's free to use.

You can execute two instances to see it sending messages to each other.


Um exemplo de como fazer uma simples comunicação usando rabbitMQ e C#.

1 - Instale o rabbitMQ em sua máquina seguindo a própria documentação da ferramenta: https://www.rabbitmq.com/download.html.

2 - Abra a solução do exemplo, compile em debug ou release.

3 - O projeto se utiliza do host localhost e a porta 5672, certifique-se que pode usá-las normalmente.

4 - Abra a pasta "bin" dentro da pasta do projeto, selecione debug ou release de acordo com sua escolha de compilação no passo 2, abra a pasta netcoreapp3.1 e execute o aplicativo "LetsTalkToCBYK"

5 - Defina um nome para a instância do MicroServiço ou utilize o nome padrão(aperte a tecla enter para usar o nome padrão).

6 - Abra outra instância da aplicação e repita o passo 5.

Prontinho, você verá as 2 instâncias conversando entre si.

