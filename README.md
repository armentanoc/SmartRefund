<!--CACHE, EVENT SOURCE, BACKGROUND SERVICE, PORTA ESPECÍFICA DA API, ESPECIFICAR TESTES-->
# Projeto SmartRefund 🤖💰
<!-- ![Front](https://img.shields.io/badge/spring-%236DB33F.svg?style=for-the-badge&logo=spring&logoColor=white) -->
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2300f.svg?style=for-the-badge&logo=mysql&logoColor=white)&nbsp;<br><br>

<table>
    <tr>
       <td style="vertical-align: top;"><img src="https://github.com/armentanoc/SmartRefund/assets/88147887/851e0a30-c8fe-4c9c-958a-cd9ac14d9667"></td>
        <td style="vertical-align: top;">Agilize o processo de reembolso de despesas utilizando técnicas avançadas de processamento de imagem. Com nossa solução, você pode extrair facilmente informações cruciais de recibos e faturas, tornando o preenchimento dos detalhes das despesas rápido, preciso e eficiente. Isso não apenas simplifica o trabalho dos colaboradores, mas também confere rapidez ao setor financeiro, permitindo que eles dediquem mais tempo a tarefas estratégicas. Simplifique seu fluxo de trabalho, elimine erros e economize tempo.</td>
    </tr>
</table>


## Endpoints da API 🚀
A API oferece os seguintes endpoints:

### Login
```
POST /[controller]: Realiza o login dos usuários no sistema. Manuseio autorizado por qualquer indivíduo.

{
  "userName": "userExample",
  "password": "passwordExample123"
}

```

<div align="center" display="flex">
<img src="" height="500px">
</div>


### Entry 

```
POST /receipt: Realiza o upload de uma imagem que é potencial nota fiscal e dá início a todo o processamento em background. Manuseio autorizado apenas para um funcinário do tipo "employee".

{
 "image": "exemplo.jpg"
}

```

<div align="center" display="flex">
<img src="" height="500px">
</div>

### Management 

```
GET /receipts/submitted: Retorna todas as notas fiscais com status em SUBMETIDO, para que o financeiro possa visualizar. Manuseio autorizado por qualquer funcionário.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

```
PATCH /receipts/status: Altera o status da despesa para PAGA ou RECUSADA. Manuseio autorizado apenas para um funcinário do tipo "finance".
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

### EventSource

```
GET {hash}/front: Busca um evento e suas entidades vinculadas pelo UniqueHash.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

```
GET /front/: Busca todos os eventos e as entidades vinculadas. 
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

```
GET {hash}/audit: Busca um evento pelo UniqueHash. 
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

## Autenticação 🔗
A API utiliza um filtro de Autorização para validar o login de funcionários. Os seguintes cargos estão disponíveis:

```
Employee - Permite que submeta notas ficais para reembolso e verifique os status das notas enviadas.

{
  "userName": "employee1",
  "password": "employee123"
}

{
  "userName": "employee2",
  "password": "employee123"
}

Finance - Possibilita visualizar todas as notas fiscais submetidas e alterar o status delas para PAGA ou RECUSADA.

{
  "userName": "finance",
  "password": "finance123"
}
```

## Estrutura do Projeto :building_construction:

A pasta `/src` contém a solução `SmartRefund` e os projetos que compõem a aplicação.

---

### 💻 `SmartRefund.WebAPI` 
Projeto principal que contém a API e os controladores.

### 📦 `SmartRefund.Domain` 
Projeto que contém as entidades de domínio da aplicação.

### 🗃️ `SmartRefund.Infra` 
Projeto responsável pela camada de infraestrutura, incluindo o contexto do banco de dados e repositórios.

### 🚀 `SmartRefund.Application` 
Projeto que implementa a lógica de aplicação e serviços.

### 👀 `SmartRefund.ViewModels` 
Projeto que contém os modelos de visualização utilizados pelos controladores.

### 🐛 `SmartRefund.CustomExceptions` 
Projeto que contém as exceções customizadas lançadas pela aplicação.

### 🧪 `SmartRefund.Tests` 
Projeto que contém os testes unitários em xUnity da lógica de negócio da aplicação.

<!--dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet tool install dotnet-stryker-->
```
dotnet test --collect:"XPlat Code Coverage"

reportgenerator "-reports:.\**\coverage.cobertura.xml" -reporttypes:Html -targetdir:output

dotnet-stryker
```

## Configurações da Aplicação Personalizadas 📁

As configurações do serviço que chama o GPT Vision para passar a imagem postada por um funcionário e extrair os dados são totalmente customizáveis: a API key é passada através de uma variável de ambiente com o nome especificado em `EnvVariable` e os `Prompts` de `System` e diversos prompts de `User` também são customizáveis, sendo traduzidos também através de um serviço de configuração, de maneira a facilitar a manutenibilidade e escalabilidade.

```
  "OpenAIVisionConfig": {
  "EnvVariable": "OPENAI_KEY_DIVERSEDEV",
  "Prompts": {
    "System": "Você é um especialista em ler notas fiscais e extrair informações importantes.",
    "User": {
      "Image": "Você deve considerar essa imagem de nota fiscal para responder às próximas perguntas.",
      "IsReceipt": "Essa imagem é algum comprovante fiscal? Responda com SIM ou NAO.",
      "Total": "Qual o valor total dessa despesa? Escreva o valor apenas com números.",
      "Category": "Que categoria de despesa é essa? Responda entre: HOSPEDAGEM OU TRANSPORTE OU VIAGEM OU ALIMENTACAO OU OUTROS.",
      "Description": "Descreva essa nota fiscal em texto corrido com detalhes como, se houver, Produto, Quantidade, Nome da Empresa, CNPJ e Data e Horário da Emissão da Nota."
    }
  }
},
```
## Autenticação na OpenAI com Variável de Ambiente🔒

- Execute o PowerShell como Administrador
- Configure a Variável de Ambiente; para uma configuração definitiva a nível de máquina, é possível fazer, por exemplo: 
```
[System.Environment]::SetEnvironmentVariable('OPENAI_KEY_DIVERSEDEV','myApiKey', 'Machine')
```
- Reinicie o Visual Studio
  
## Middleware Customizado de Logging 🗞️ e Filtro Customizado de Exceção 🐛

Através do `Middlewares/LoggingMiddleware` é realizado o logging sempre no começo e no final de uma requisição, com detalhes sobre o status e eventuais erros de forma personalizada, que são capturados no Filtro Customizado de Exceção Global (`Filters/ExceptionFilter.cs`).

<div align="center" display="flex">
<img src="..." height="500px">
</div>
<br/>

<div align="center" display="flex">
    
| Código | Descrição do erro |
|---|---|
| `200` | Requisição executada com sucesso (Success).|
| `400` | Operação inválida (InvalidOperation).|
| `400` | Status do internalReceipt incompatível com o ChatGPT.|
| `400` | Nota fiscal com a identificação requisitada já foi atualizada.|
| `400` | Não conseguiu converter o valor para o tipo requisitado.|
| `404` | Registro pesquisado não encontrado (Not found).|
| `409` | Entidade com as propriedades descritas já existe.|
| `412` | Configuração da propriedade não pode ser nula, vazia ou inválida.|
| `413` | Tamanho do arquivo inválido, possui mais que 20MB.|
| `422` | Tipo do arquivo inválido, extensão diferente de .png/.jpg/.jpeg|
| `422` | Resulução do arquivo inválida, sendo abaixo da PPI requisitada.|
| `500` | Outros tipos de exceções.|

</div>

## Especificação de testes 📋

### Teste de carga

Foram realizados testes de carga para verificar o desempenho da aplicação, utilizando um escopo que varia de 5 a 30 usuários virtuais simultâneos no endpoint GET: api/management/submitted.

![Teste de Carga](https://drive.google.com/uc?id=1yXhp445NGhlrA8Gz71cs9UxGUXzv8fzT) <br/><br/>
**Data Received / Data Sent:** Durante o teste, o servidor recebeu um total de 1.0 MB de dados a uma taxa média de 9.2 kB/s. Além disso, foram enviados 144 kB de dados a uma taxa média de 1.3 kB/s.<br/>
**HTTP Request Blocked:** Esta métrica mostra o tempo médio que uma solicitação HTTP ficou bloqueada antes de ser enviada. O tempo médio foi de 25.58 µs, com 95% das solicitações sendo bloqueadas por menos de 0.01 ms.<br/>
**HTTP Request Connecting:** Mostra o tempo médio necessário para estabelecer a conexão TCP. O tempo médio foi de 16.88 µs, com 95% das conexões sendo estabelecidas em menos de 2.95 ms.<br/>
**HTTP Request Duration:** Esta métrica indica a duração média de uma solicitação HTTP, desde o início da requisição até o recebimento da resposta. A duração média foi de 2.35 ms, com 95% das solicitações completadas em menos de 5.77 ms.<br/>
**HTTP Request Failed:** Nenhuma solicitação falhou durante o teste, o que é um bom sinal. Todas as 1421 solicitações foram concluídas com sucesso.<br/>
**HTTP Request Receiving:** Indica o tempo médio que o k6 esperou pela resposta do servidor. O tempo médio foi de 73.23 µs, com 95% das respostas sendo recebidas em menos de 505.8 µs.<br/>
**HTTP Request Sending:** Indica o tempo médio gasto para enviar a requisição ao servidor. O tempo médio foi de 11.14 µs, com 95% das solicitações sendo enviadas em menos de 0 µs.<br/>
**HTTP Request Waiting:** Indica o tempo médio que o k6 esperou entre o envio da requisição e o recebimento da primeira resposta do servidor. O tempo médio foi de 2.27 ms, com 95% das solicitações sendo atendidas em menos de 5.66 ms.<br/>
**HTTP Requests:** Durante o teste, foram feitas 1421 solicitações HTTP, com uma taxa média de 12.89 solicitações por segundo.<br/>
**Iteration Duration:** Cada iteração do teste (um ciclo completo de todas as solicitações) teve uma duração média de 1.01 segundos, com 95% das iterações durando menos de 1.01 segundos.

Também foram realizados testes de carga simultâneos no endpoint POST: api/receipt e GET: api/management/submitted com um escopo variavel de 1 a 10 usuários.

![Teste de Carga](https://drive.google.com/uc?id=1RomCgs-azt_GtEQeYZYdHNiBgb26ZPgb)<br/><br/>
**Data Received / Data Sent**: Durante o teste, o servidor recebeu um total de 123 kB de dados a uma taxa média de 5.3 kB/s. Além disso, foram enviados 5.5 MB de dados a uma taxa média de 236 kB/s.<br/>
**HTTP Request Blocked**: O tempo médio que uma solicitação HTTP ficou bloqueada antes de ser enviada foi de 159.13 µs, com 95% das solicitações sendo bloqueadas em menos de 724.66 µs.<br/>
**HTTP Request Connecting**: O tempo médio necessário para estabelecer a conexão TCP foi de 67.16 µs, com 95% das conexões sendo estabelecidas em menos de 605.4 µs.<br/>
**HTTP Request Duration**: A duração média de uma solicitação HTTP, desde o início até o recebimento da resposta, foi de 846.94 ms. A maioria das solicitações (95%) foi concluída em menos de 3.5 segundos.<br/>
**HTTP Request Failed**: Durante o teste, nenhuma solicitação falhou, o que é um ótimo sinal. Todas as 114 solicitações foram concluídas com sucesso.<br/>
**HTTP Request Receiving**: O tempo médio que o K6 esperou pela resposta do servidor foi de 761.2 µs, com 95% das respostas sendo recebidas em menos de 4.87 ms.<br/>
**HTTP Request Sending**: O tempo médio gasto para enviar a solicitação ao servidor foi de 643.29 µs, com 95% das solicitações sendo enviadas em menos de 1 ms.<br/>
**HTTP Request Waiting**: O tempo médio que o K6 esperou entre o envio da solicitação e o recebimento da primeira resposta do servidor foi de 845.54 ms, com 95% das solicitações sendo atendidas em menos de 3.48 segundos.<br/>
**HTTP Requests**: Durante o teste, foram feitas 114 solicitações HTTP, com uma taxa média de 4.88 solicitações por segundo.<br/>
**Iteration Duration**: Cada iteração do teste (um ciclo completo de todas as solicitações) teve uma duração média de 2.7 segundos.<br/>
**Iterations**: Durante o teste, ocorreram 57 iterações, com uma taxa média de 2.44 iterações por segundo.<br/>

### Teste de Stress

Foram realizados testes de stress, utilizando um escopo de 5 a 300 usuários simultâneos.

![Teste de Stress](https://drive.google.com/uc?id=1XqkGjdN6Vmnx4qFknsZSVJFDjFrG6lzh) <br/><br/>

**Data Received / Data Sent:** Durante o teste, o servidor recebeu um total de 12 MB de dados a uma taxa média de 83 kB/s. Além disso, foram enviados 1.7 MB de dados a uma taxa média de 12 kB/s. Isso indica um volume significativo de comunicação de dados entre o cliente e o servidor. <br/>
**HTTP Request Blocked:** O tempo médio que uma solicitação HTTP ficou bloqueada antes de ser enviada foi de 20.67 µs, com 95% das solicitações sendo bloqueadas por menos de 0.01 ms. Isso mostra uma eficiência na preparação das solicitações para envio. <br/>
**HTTP Request Connecting:** O tempo médio necessário para estabelecer a conexão TCP foi de 18.11 µs, com 95% das conexões sendo estabelecidas em menos de 0.01 ms. Isso indica uma rápida conexão e preparação para o envio das solicitações. <br/>
**HTTP Request Duration:** A duração média de uma solicitação HTTP, desde o início até o recebimento da resposta, foi de 1.81 ms. A maioria das solicitações (95%) foi concluída em menos de 4.93 ms. Isso sugere um desempenho relativamente estável do servidor, mesmo sob carga elevada. <br/>
**HTTP Request Failed:** Durante o teste, nenhuma solicitação falhou, o que é um ótimo sinal. <br/>
**HTTP Request Receiving:** O tempo médio que o k6 esperou pela resposta do servidor foi de 37.23 µs, com 95% das respostas sendo recebidas em menos de 0.39 ms. <br/>
**HTTP Request Sending:** O tempo médio gasto para enviar a solicitação ao servidor foi de 9.94 µs, com 95% das solicitações sendo enviadas em menos de 0 ms.<br/>
**HTTP Request Waiting:** O tempo médio que o k6 esperou entre o envio da solicitação e o recebimento da primeira resposta do servidor foi de 1.77 ms, com 95% das solicitações sendo atendidas em menos de 4.91 ms.<br/>
**HTTP Requests:** Durante o teste, foram feitas 17039 solicitações HTTP, com uma taxa média de 116.73 solicitações por segundo. Isso mostra uma alta carga de solicitações durante o teste.<br/>
**Iteration Duration:** Cada iteração do teste (um ciclo completo de todas as solicitações) teve uma duração média de 1.01 segundos, com 95% das iterações durando menos de 1.01 segundos.

### Teste de Imersão/Soak Test

Foram realizados testes de imersão, o teste durou 1 hora e 16 minutos, utilizando de 0 a 10 usuários virtuais simultâneos.

![Teste de Imersão](https://drive.google.com/uc?id=1x7aR7kLNEVtnNy6iVt_hnAuFdEstId6-)

**Data Received / Data Sent:** Durante o teste, o servidor recebeu um total de 24 MB de dados a uma taxa média de 5.3 kB/s. Além disso, foram enviados 3.4 MB de dados a uma taxa média de 751 B/s. <br/>
**HTTP Request Blocked:** O tempo médio que uma solicitação HTTP ficou bloqueada antes de ser enviada foi de 1.73 µs, com 95% das solicitações sendo bloqueadas por menos de 0.01 ms.<br/>
**HTTP Request Connecting:** O tempo médio necessário para estabelecer a conexão TCP foi de 187 ns, com 95% das conexões sendo estabelecidas em menos de 0.01 ms.<br/>
**HTTP Request Duration:** A duração média de uma solicitação HTTP, desde o início até o recebimento da resposta, foi de 1.67 ms. A maioria das solicitações (95%) foi concluída em menos de 9.37 ms.<br/>
**HTTP Request Failed:** Durante o teste, nenhuma solicitação falhou, o que é um ótimo sinal. Todas as 33908 solicitações foram concluídas com sucesso.<br/>
**HTTP Request Receiving:** O tempo médio que o k6 esperou pela resposta do servidor foi de 59.32 µs, com 95% das respostas sendo recebidas em menos de 378.1 µs.<br/>
**HTTP Request Sending:** O tempo médio gasto para enviar a solicitação ao servidor foi de 4.26 µs, com 95% das solicitações sendo enviadas em menos de 0 µs.<br/>
**HTTP Request Waiting:** O tempo médio que o k6 esperou entre o envio da solicitação e o recebimento da primeira resposta do servidor foi de 1.61 ms, com 95% das solicitações sendo atendidas em menos de 9.28 ms.<br/>
**HTTP Requests:** Durante o teste, foram feitas 33908 solicitações HTTP, com uma taxa média de 7.44 solicitações por segundo.<br/>
**Iteration Duration:** Cada iteração do teste (um ciclo completo de todas as solicitações) teve uma duração média de 1 segundo.<br/>

Este teste de imersão mostra como o servidor se comporta sob uma carga sustentada por um longo período. Todos os indicadores estão dentro dos limites desejados, sem falhas e com tempos de resposta aceitáveis. Isso sugere que o servidor é capaz de lidar com a carga esperada e permanecer estável ao longo do tempo.

### Teste de cobertura

```
dotnet test --collect:"XPlat Code Coverage"

reportgenerator "-reports:.\**\coverage.cobertura.xml" -reporttypes:Html -targetdir:output

dotnet-stryker
```

## Configuração do Banco de Dados 🛢️

O projeto utiliza o SQLite como banco de dados, e as configurações podem ser encontradas no arquivo `appsettings.json` do projeto `SmartRefund.WebAPI`. Certifique-se de ajustar as configurações conforme necessário.

```json
"ConnectionStrings": {
  "SmartRefundSqlite": "Data Source=SmartRefundDB.db"
},
```
<!--<div align="center" display="flex">
<img src="" height="500px">
</div>
-->

## Execução do Projeto ▶️

1. Clone e abra a solução no Visual Studio.
2. Configure o projeto `SmartRefund.Infra` como o projeto de inicialização no `Package Manager Console`.
3. Certifique-se de que as migrações do banco de dados foram realizadas pelo Entity Framework. Se não, execute os seguintes comandos:
```
Add-Migration CreateDatabaseInitial
Update-Database
```
4. Execute o projeto na sua máquina.
5. Abra o link da interface para [login](https://smart-refund-front.vercel.app/login).

## Documentação da API 📚
A API está documentada usando Swagger. Após a execução do projeto, acesse a documentação em:

```
http://localhost:xxxx/swagger/v1/swagger.json
```

## Contribuições 🛠️

Aceitamos contribuições! Se encontrar um bug ou tiver uma solicitação de recurso, por favor, abra uma issue. 

## Autores 📖

| [<img loading="lazy" src="https://github.com/lauradefaria/Extras/blob/main/Imagens/laura.jpg" width=115><br><sub>Laura de Faria</sub>](https://github.com/lauradefaria) |  [<img loading="lazy" src="https://github.com/lauradefaria/Extras/blob/main/Imagens/carolina.jpg" width=115><br><sub>Carolina Armentano</sub>](https://github.com/armentanoc) |  [<img loading="lazy" src="https://github.com/lauradefaria/Extras/blob/main/Imagens/camila.jpg" width=115><br><sub>Camila Zambini</sub>](https://github.com/czambanini) | [<img loading="lazy" src="https://github.com/lauradefaria/Extras/blob/main/Imagens/paula.jpg" width=115><br><sub>Paula Andrezza</sub>](https://github.com/paulaandrezza) | [<img loading="lazy" src="https://github.com/lauradefaria/Extras/blob/main/Imagens/igor.jpg" width=115><br><sub>Igor Nunes</sub>](https://github.com/ig-nunes) | [<img loading="lazy" src="https://github.com/lauradefaria/Extras/blob/main/Imagens/christopher.jpg" width=115><br><sub>Cristopher Saporiti</sub>](https://github.com/cristopherkovalski)
| :---: | :---: | :---: | :---: | :---: | :---: |



