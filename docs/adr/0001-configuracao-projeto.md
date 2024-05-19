# ADR 0001: Configuração do Projeto

## Contexto

Para desenvolver uma aplicação de API Web em .NET seguindo princípios de SOLID, boas práticas de design e integração com serviços externos, é necessário definir uma arquitetura clara e modular. A aplicação deve incluir um código de boleto válido, processar apenas boletos do tipo NPC que estejam vencidos, calcular juros e multas, e consumir uma API externa para obter informações dos boletos.

## Decisão

Decidimos estruturar o projeto em uma solução .NET com os seguintes projetos:

1. **API Web**: Projeto principal que expõe endpoints REST.
2. **Domain**: Contém as entidades e regras de negócio.
3. **Infrastructure**: Gerencia a comunicação com o banco de dados e outros serviços externos.
4. **Service**: Contém a lógica de aplicação e orquestração.

Além disso, implementamos um middleware para gerenciamento de tokens de autenticação e configuramos a aplicação para utilizar o banco de dados SQLite para armazenar cálculos de boletos.

## Consequências

### Positivas

- **Modularidade**: A separação de responsabilidades em projetos diferentes melhora a manutenção e a escalabilidade do sistema.
- **Flexibilidade**: Facilidade para substituir ou atualizar componentes específicos sem afetar outras partes do sistema.
- **Clareza**: A arquitetura modular facilita a compreensão e a colaboração entre desenvolvedores.

### Negativas

- **Complexidade Inicial**: Configuração inicial mais complexa devido à divisão em múltiplos projetos e configuração de injeção de dependência.

## Decisões Futuras

Considerar a implementação de um pipeline de CI/CD para automação de testes e deploy, bem como a expansão da cobertura de testes automatizados para garantir a robustez do sistema.

---

### Registro de Decisões

- **Data**: 17/05/2024
- **Autores**: Arthur Rodrigues Dias
