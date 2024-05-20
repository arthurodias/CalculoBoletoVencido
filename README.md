# Calcular Boleto Vencido

## Visão Geral

Esta API foi desenvolvida para calcular pagamentos de boletos vencidos, garantindo conformidade com regras de negócio específicas e interações com uma API externa. A aplicação está estruturada para ser escalável, manutenível e testável, seguindo os princípios SOLID e utilizando boas práticas de design.

## Estrutura do Projeto

- **BoletoApi**: Projeto principal que expõe a API RESTful.
- **BoletoDomain**: Contém as entidades e lógica de negócios.
- **BoletoInfrastructure**: Responsável pelo acesso a dados e comunicação com APIs externas.
- **BoletoService**: Implementa a lógica de negócios e serviços.
- **BoletoTests**: Contém testes unitários e de integração.

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- SQLite
- Moq
- xUnit

## Configuração do Banco de Dados

A aplicação utiliza SQLite como banco de dados. A configuração do banco de dados é feita no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=BoletoDb.db"
  }
}
```

## Configuração do Projeto

### 1. Clonar o Repositório

```
git clone https://github.com/arthurodias/CalculoBoletoVencido.git
cd CalculoBoletoVencido
```
### 2. Restaurar Dependências
```
dotnet restore
```
### 3. Configurar as Credenciais da API de Boletos
No arquivo appsettings.json, configure as credenciais da API:
```json
{
  "BoletoApi": {
    "ClientId": "bd753592-cf9b-4d1a-96b9-cb8b2c01bd12",
    "ClientSecret": "4e8229fe-1131-439c-9846-799895a8be5b"
  }
}
```
### 4. Migrações do Entity Framework

Antes de rodar a aplicação ou os testes, é necessário aplicar as migrações para garantir que o esquema do banco de dados está atualizado.

### Aplicando Migrações
Abra o terminal na raiz do projeto.
Execute o comando:
```
dotnet ef database update
```
Este comando aplicará todas as migrações pendentes ao banco de dados configurado.

Adicionando uma Nova Migração
Se você fizer alterações no modelo de dados e precisar criar uma nova migração, execute:

```
dotnet ef migrations add NomeDaMigracao
dotnet ef database update
```

### 5. Executar a Aplicação
```
dotnet run --project Boleto.Api
```
A aplicação estará disponível em http://localhost:5075.

Endpoints da API

Calcular Boleto Vencido
Endpoint: POST /api/boleto

Request:

```json
{
  "bar_code": "string",
  "payment_date": "string"
}
```
Response:

```json
{
  "original_amount": 0,
  "amount": 0,
  "due_date": "string",
  "payment_date": "string",
  "interest_amount_calculated": 0,
  "fine_amount_calculated": 0
}
```
Exemplo de Uso
```
curl -X 'POST' \
  'http://localhost:5075/Boleto' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "bar_code": "34191790010104351004791020150008291070026000",
  "payment_date": "20/05/2024"
}'
```
### 6. Testes

Executar Testes Unitários e de Integração
```
dotnet test
```
Estrutura de Diretórios
```
BoletoSolution/
├── docs/
│   └── adr/
├── src/
│   ├── Boleto.API
│   ├── Boleto.Domain
│   ├── Boleto.Infrastructure
│   └── Boleto.Service
├── tests/
│   ├── Boleto.Tests/
│   │   ├── UnitTests
│   │   ├── IntegrationTests
├── .gitignore
├── README.md
└── BoletoSolution.sln
```
### Autor
Arthur Rodrigues Dias - arthur.rodias@gmail.com - https://github.com/arthurodias

