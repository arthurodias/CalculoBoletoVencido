# ADR 002: Integração com API Externa

## Contexto:
A aplicação precisa buscar informações de boletos de uma API externa. Isso requer o gerenciamento de autenticação e a realização de solicitações HTTP para a API.

## Decisão:
Decidimos implementar um cliente de API dedicado na camada Infrastructure para lidar com a comunicação com a API externa de boletos. Este cliente será responsável por gerenciar tokens de autenticação e fazer solicitações ao serviço externo.

## Justificativa:

- **Princípio da Responsabilidade Única**: Ao isolar a comunicação com a API externa em um cliente dedicado, aderimos ao SRP, garantindo que o restante da aplicação permaneça focado na lógica de negócios.
- **Reusabilidade**: O cliente de API pode ser reutilizado ou substituído independentemente do restante da aplicação.
- **Encapsulamento**: Encapsular os detalhes da comunicação com a API em uma única classe torna a base de código mais limpa e manutenível.

## Consequências:

### Positivas:

Melhor organização e legibilidade do código.
Facilita a gestão de mudanças relacionadas à comunicação com a API externa.
Simplificação dos testes, permitindo implementações simuladas (mock) do cliente de API.

### Negativas:

Complexidade adicional no gerenciamento de tokens de autenticação.
Potencial para aumento de latência devido à comunicação em rede com a API externa.

## Alternativas Consideradas:

Chamadas Diretas à API na Camada de Serviço: Rejeitado devido ao potencial de duplicação de código e má separação de responsabilidades.
Bibliotecas de Terceiros: Usar bibliotecas de terceiros para comunicação com a API foi considerado, mas julgado desnecessário para o escopo deste projeto.
Implementação:

Implementado BoletoApiClient no projeto BoletoService.
Configurado o cliente HTTP com injeção de dependência no Program.cs.
Adicionado middleware para gerenciamento de tokens para lidar com autenticação de API.

## Status:
Aceito e implementado.

Data:
19-05-2024
