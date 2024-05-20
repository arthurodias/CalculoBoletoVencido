# ADR 001: Estrutura e Design do Projeto

## Contexto:
Precisamos projetar um projeto .NET para calcular pagamentos de boletos vencidos, garantindo conformidade com regras de negócio específicas e interações com uma API externa. O projeto deve seguir as melhores práticas para manutenibilidade, escalabilidade e aderência aos princípios SOLID.

## Decisão:
Decidimos estruturar o projeto em camadas e projetos separados para melhor separação de responsabilidades e manutenibilidade:

- **Projeto Domain**: Contém a lógica de negócios e entidades.
- **Projeto Infrastructure**: Lida com acesso a dados e interações com APIs externas.
- **Projeto Service**: Implementa a lógica de negócios e serviços da aplicação.
- **Projeto Web API**: Expõe a funcionalidade via API RESTful.
- **Projeto Test**: Contém testes unitários e de integração.
  
## Justificativa:

- **Separação de Responsabilidades**: Dividir o projeto em camadas garante que cada camada tenha uma única responsabilidade, alinhando-se com o Princípio da Responsabilidade Única (SRP) do SOLID.
- **Manutenibilidade**: A separação clara permite uma manutenção e atualização mais fáceis. Mudanças em uma camada não afetam diretamente as outras.
- **Testabilidade**: Esta estrutura facilita a escrita de testes unitários e de integração, permitindo a simulação (mocking) de dependências.
- **Escalabilidade**: A arquitetura permite escalar cada camada de forma independente, se necessário.

## Consequências:

### Positivas:

- **Melhor legibilidade e organização do código.**
- **Facilita a integração de novos desenvolvedores.**
- **Simplificação dos processos de teste e depuração.**
- **Flexibilidade para substituir ou atualizar componentes individuais (por exemplo, trocar o banco de dados).**

### Negativas:

- **A configuração inicial é mais complexa devido à necessidade de gerenciar múltiplos projetos e dependências.**
- **Potencial de superengenharia se não for gerenciado adequadamente, especialmente para projetos menores.**

## Alternativas Consideradas:

- **Abordagem Monolítica**: Manter tudo em um único projeto. Rejeitado devido a preocupações com manutenibilidade e escalabilidade.
- **Microserviços**: Dividir em serviços menores para cada funcionalidade. Considerado muito complexo para os requisitos atuais e escopo do projeto.

## Implementação:

- **Criada uma solução com quatro projetos: BoletoApi, BoletoDomain, BoletoInfrastructure e BoletoService.**
- **Adicionado um quinto projeto, BoletoTests, para testes.**
- **Usado injeção de dependência para gerenciar dependências entre camadas.**
- **Implementado um cliente de API externa na camada Infrastructure para buscar dados de boletos.**
- **Configurado o Program.cs para usar configurações modernas do .NET sem o arquivo Startup.cs.**
- **Adicionado middleware para gerenciamento de tokens para autenticação de API.**

## Status:
Aceito e implementado.

Data:
19-05-2024
