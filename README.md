# Boleto API

## Visão Geral

Esta API foi desenvolvida para calcular pagamentos de boletos vencidos, garantindo conformidade com regras de negócio específicas e interações com uma API externa. A aplicação está estruturada para ser escalável, manutenível e testável, seguindo os princípios SOLID e utilizando boas práticas de design.

## Estrutura do Projeto

- **BoletoApi**: Projeto principal que expõe a API RESTful.
- **BoletoDomain**: Contém as entidades e lógica de negócios.
- **BoletoInfrastructure**: Responsável pelo acesso a dados e comunicação com APIs externas.
- **BoletoService**: Implementa a lógica de negócios e serviços.
- **BoletoTests**: Contém testes unitários e de integração.

## Requisitos

- .NET 8.0 SDK ou superior

## Configuração do Projeto

### 1. Clonar o Repositório

git clone https://github.com/arthurodias/CalculoBoletoVencido.git
cd CalculoBoletoVencido

2. Restaurar Dependências
bash
Copiar código
dotnet restore

3. Configurar as Credenciais da API de Boletos
No arquivo appsettings.json, configure as credenciais da API:

json
Copiar código
{
  "ApiSettings": {
    "ClientId": "bd753592-cf9b-4d1a-96b9-cb8b2c01bd12",
    "ClientSecret": "4e8229fe-1131-439c-9846-799895a8be5b",
    "AuthUrl": "https://vagas.builders/api/builders/auth/tokens",
    "BoletoUrl": "https://vagas.builders/api/builders/bill-payments/codes"
  }
}
4. Executar a Aplicação
bash
Copiar código
dotnet run --project BoletoApi
A aplicação estará disponível em http://localhost:5000.

Endpoints da API

Calcular Boleto Vencido
Endpoint: POST /api/boletos/calculate

Request:

json
Copiar código
{
  "bar_code": "string",
  "payment_date": "string"
}
Response:

json
Copiar código
{
  "original_amount": 0,
  "amount": 0,
  "due_date": "string",
  "payment_date": "string",
  "interest_amount_calculated": 0,
  "fine_amount_calculated": 0
}
Exemplo de Uso
bash
Copiar código
curl --location --request POST 'http://localhost:5000/api/boletos/calculate' \
--header 'Content-Type: application/json' \
--data-raw '{
    "bar_code": "34191790010104351004791020150008291070026000",
    "payment_date": "2023-05-10"
}'
Testes

Executar Testes Unitários e de Integração
bash
Copiar código
dotnet test
Estrutura de Diretórios

Copiar código
.
├── BoletoApi
│   ├── Controllers
│   ├── Program.cs
│   ├── appsettings.json
│   └── ...
├── BoletoDomain
│   ├── Entities
│   ├── Interfaces
│   └── ...
├── BoletoInfrastructure
│   ├── Clients
│   ├── Repositories
│   └── ...
├── BoletoService
│   ├── Services
│   └── ...
├── BoletoTests
│   ├── UnitTests
│   ├── IntegrationTests
│   └── ...
└── ...
Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests para melhorias e correções.

Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

Autor
Arthur Rodrigues Dias - arthur.rodias@gmail.com - https://github.com/arthurodias

