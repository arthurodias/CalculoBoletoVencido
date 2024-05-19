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

```bash
git clone https://github.com/arthurodias/CalculoBoletoVencido.git
cd CalculoBoletoVencido
