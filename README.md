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

## Authentication 🔗
A API utiliza um filtro de Autorização para validar o login de funcionários. Os seguintes cargos estão disponíveis:

```
Employee - Permite que submeta notas ficais para reembolso e verifique os status das notas enviadas.

{
  "userName": "employee1",
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
<!-- Baixar o arquivo para executar o front-->
1. Clone e abra a solução no Visual Studio.
2. Configure o projeto `SmartRefund.Infra` como o projeto de inicialização no `Package Manager Console`.
3. Certifique-se de que as migrações do banco de dados foram realizadas pelo Entity Framework. Se não, execute os seguintes comandos:
```
Add-Migration CreateDatabaseInitial
Update-Database
```
4. Execute o projeto.

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



