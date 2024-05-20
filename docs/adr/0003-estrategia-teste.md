ADR 003: Estratégia de Testes

Contexto:
Para garantir a confiabilidade e a correção da aplicação, precisamos de uma estratégia de testes robusta que inclua testes unitários e testes de integração.

Decisão:
Decidimos usar o framework de testes xUnit e a biblioteca Moq para criar objetos simulados (mock). Os testes serão divididos em testes unitários para componentes individuais e testes de integração para a funcionalidade de ponta a ponta.

Justificativa:

Cobertura de Testes: Garante que tanto os componentes individuais quanto todo o sistema sejam testados minuciosamente.
Manutenibilidade: Usar mocks ajuda a isolar dependências, facilitando o teste de componentes de forma isolada.
Confiabilidade: Testes de integração ajudam a verificar que diferentes componentes do sistema funcionam juntos conforme esperado.
Consequências:

Positivas:

Alta confiança na qualidade e correção do código.
Detecção precoce de bugs e problemas.
Refatoração facilitada devido à cobertura de testes confiável.
Negativas:

Aumento do tempo inicial de desenvolvimento devido à necessidade de escrever e manter testes.
Potencial para testes inconsistentes se não forem gerenciados adequadamente.
Alternativas Consideradas:

Testes Manuais: Rejeitado devido à incapacidade de reproduzir testes consistentemente e maior chance de perder bugs.
Sem Testes de Integração: Apenas usar testes unitários foi considerado, mas rejeitado devido à falta de verificação de ponta a ponta.
Implementação:

Criado projeto BoletoTests.
Adicionados testes unitários para serviços usando Moq.
Adicionados testes de integração usando xUnit e Moq para verificar a funcionalidade de ponta a ponta.
Configuradas respostas simuladas para chamadas de API externa nos testes de integração.
Status:
Aceito e implementado.

Data:
19-05-2024