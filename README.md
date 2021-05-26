# ContaAPI
 
Projeto que simula um sistema de conta corrente, no qual cada usuário tem uma carteira e pode fazer operações de depósito, retirada e pagamento por PIX. Também dentro do sistema existe uma rentabilização diária no valor de 100% do CDI para todas as carteiras, assim como o histórico de operações.

O projeto tem duas camadas:

- Uma API RESTful com todos os serviços que foi desenvolvida em ASP.NET Core 3.1 com arquitetura DDD (Domain Driven Design), sendo o banco de dados utilizado MySQL com EF Core como ORM;
- Um aplicativo web em MVC com uma interface visual para facilitar o uso e os testes do sistema.

## Rodando no Visual Studio

Para testar o projeto no Visual Studio basta seguir os seguintes passos:

- Definir como **Multiple Startup Projects** os projetos ContaAPI.Application e ContaAPI.WebMVC nas propriedades da solution;
- Definir os dados do servidor e DB a ser utilizado no arquivo **appsettings.json** dentro do projeto ContaAPI.Application;
- No **Package Manager Console** definir como **Default Project** o projeto ContaAPI.Infra.Data e executar o comando **Update-Database**.

Ao executar estes passos, duas janelas irão se abrir: uma do aplicativo web MVC e a outra do swagger.

- Aplicativo Web MVC - http://localhost:51359
- Swagger (API REST) - https://localhost:5001

## API RESTful

### Usuário - Funcionamento

As chamadas relativas ao controle de usuário são as seguintes:

1. **Criação de usuário** - [POST] /api/users

Cria um usuário com os dados de nome, email e senha. Retorna um erro se o email já está sendo utilizado ou se algum campo é inválido. Automaticamente cria uma conta corrente com saldo R$0 para o usuário.

2. **Retorno de todos usuários** - [GET] /api/users

Retorna uma lista de JSON com os dados de todos usuários cadastrados.

3. **Alterar dados de um usuário** - [PUT] /api/users/{id}

Altera os dados do usuário através do seu {id}. Podem ser alterados o nome, email e senha. A propriedade que estiver com uma string em branco não será alterada. Retorna um erro se o email já está sendo utilizado ou se algum campo é inválido.

4. **Deleção de um usuário** - [DELETE] /api/users/{id}

Deleta um usuário através do seu {id}.

5. **Retorno de um usuário** - [GET] /api/users/{id}

Retorna um JSON com os dados de um usuário através do seu {id}.

6. **Login de usuário** - [POST] /api/users/login

Retorna uma resposta de sucesso se os dados de email e senha no login estão corretos.

### Usuário - Validações

Os valores do tipo **nome**, **email** e **senha** são validados seguindo as regras:

- O **nome** deve ter no mínimo 3 caracteres, começar com uma letra, não ter nenhum caracter especial e também não acabar com espaço em branco;
- O **email** deve estar no formato padrão (string@string.string);
- A **senha** deve ter no mínimo 6 caracteres, uma letra e um número.

### Conta Corrente - Funcionamento

As chamadas relativas ao controle de conta corrente são as seguintes:

1. **Depósito** - [PUT] /api/accounts/deposit/{userId}

Adiciona ao saldo da conta corrente do usuário de id {userId} o valor informado.

2. **Retirada** - [PUT] /api/accounts/withdraw/{userId}

Subtrai do saldo da conta corrente do usuário de id {userId} o valor informado.

3. **Pagamento** - [PUT] /api/accounts/payment/{userId}

Subtrai do saldo da conta corrente do usuário de id {userId} o valor informado, devendo o campo do código PIX ser válido.

4. **Rentabilização** - [POST] /api/accounts/monetize

Adiciona ao saldo de todas as contas correntes o valor referente a 100% do CDI diário, arredondando o resultado para 2 casas decimais. Este valor foi calculado tomando como base o rendimento do CDI no ano de 2020 e, fazendo-se uma média diário chegou-se no valor de 0,00756%.

5. **Retorno das contas** - [GET] /api/accounts

Retorna uma lista de JSON com os dados de todas as contas correntes.

6. **Retorno de uma conta** - [GET] /api/accounts/{userId}

Retorna um JSON com os dados da conta corrente do usuário de id {userId}.

7. **Histórico de todas as contas** - [GET] /api/historic

Retorna uma lista de JSON com o histórico de operações de todas as contas correntes.

8. **Histórico de uma conta** - [GET] /api/historic/{userId}

Retorna uma lista de JSON com o histórico de operações da conta corrente do usuário de id {userId}.

### Conta Corrente - Validações

O valor informado em qualquer operação deve ser positivo, maior que zero e ter no máximo 2 casas decimais. O código PIX, por sua vez, para ser válido deve estar em algum dos seguintes formatos:

- Email: validado assim como nos serviçoes de usuário;
- CPF: 11 números ou xxx.xxx.xxx-xx;
- Telefone: 11 números ou (xx)xxxxx-xxxx;
- Chave aleatória: 11 caracteres, podendo ser letras, números ou hífen.

## Aplicativo Web MVC

O aplicativo web MVC desenvolvido tem as funções de **login**, **logout**, **registro**, **saldo**, **depósito**, **retirada**, **pagamento**, **rentabilização** e **histórico** com sessão persistente e mensagens de erro. O botão **Passar 1 dia** simula um dia de rentabilização com 100% do CDI.

## Teste

O projeto ContaAPI.IntegrationTests testa todas as operações válidas e a integração com o database.

## Observações

Ideias do que pode ser implementado ainda no projeto:

- Sistema de transferência entre contas correntes;
- Funções de registro e deleção de usuário no aplicativo web MVC;
- Migration automática;
- Sistema de autenticação por JWT.