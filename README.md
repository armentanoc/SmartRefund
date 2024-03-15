<!--CACHE, EVENT SOURCE, BACKGROUND SERVICE, PORTA ESPECÍFICA DA API, ESPECIFICAR TESTES-->
[![author](https://img.shields.io/badge/author-lauradefaria-blue.svg)](https://github.com/lauradefaria)
[![author](https://img.shields.io/badge/author-armentanoc-black.svg)](https://github.com/armentanoc)
[![author](https://img.shields.io/badge/author-czambanini-purple.svg)](https://github.com/czambanini)
[![author](https://img.shields.io/badge/author-paulaandrezza-pink.svg)](https://github.com/paulaandrezza)
[![author](https://img.shields.io/badge/author-ignunes-green.svg)](https://github.com/ig-nunes)
[![author](https://img.shields.io/badge/author-cristopherkovalski-red.svg)](https://github.com/cristopherkovalski)
# Projeto SmartRefund 🤖💰

<table>
    <tr>
       <td style="vertical-align: top;"><img src="https://github.com/armentanoc/SmartRefund/assets/88147887/851e0a30-c8fe-4c9c-958a-cd9ac14d9667"></td>
        <td style="vertical-align: top;">Agilize o processo de reembolso de despesas utilizando técnicas avançadas de processamento de imagem. Com nossa solução, você pode extrair facilmente informações cruciais de recibos e faturas, tornando o preenchimento dos detalhes das despesas rápido, preciso e eficiente. Isso não apenas simplifica o trabalho dos colaboradores, mas também confere rapidez ao setor financeiro, permitindo que eles dediquem mais tempo a tarefas estratégicas. Simplifique seu fluxo de trabalho, elimine erros e economize tempo.</td>
    </tr>
</table>


## Endpoints da API 🚀
A API oferece os seguintes endpoints:

### Entry 🔗

```
POST /employeeId
{
 "image": "exemplo.jpg"
}

Realiza o upload de uma imagem que é potencial nota fiscal e dá início a todo o processamento em background.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

### Management (icone)

```
GET /receipts/submitted
{
 //
}

Retorna todas as notas fiscais com status em SUBMETIDO, para que o financeiro possa visualizar.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

```
PATCH /update-status
{
 //
}

Altera o status da despesa para PAGA ou RECUSADA.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

```
GET /receipts
{
 //
}

Retorna todas as notas fiscais existentes no sistema.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

### Test (icone)

```
GET /get/{id}
{
 //
}

Retorna a nota fiscal desejada pelo ID.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

```
GET /executeVision/{id}
{
 //
}

Seleciona a possível nota fiscal pelo ID, depois realiza a verificação e execução pelo ChatGPT, retornando o resultado fornecido sem nenhuma alteração.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>

```
GET /testaTraducao/{id}
{
 //
}

Seleciona pelo ID uma nota fiscal já executada pela API do ChatGPT e reformula a resposta armazenada para o formato desejado da saída.
```

<div align="center" display="flex">
<img src="" height="500px">
</div>


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
4. Execute o projeto.

## Documentação da API 📚
A API está documentada usando Swagger. Após a execução do projeto, acesse a documentação em:

```
http://localhost:xxxx/swagger/v1/swagger.json
```

## Contribuições 🛠️

Aceitamos contribuições! Se encontrar um bug ou tiver uma solicitação de recurso, por favor, abra uma issue. 


--------------------------

# Nossas anotações

## Git Co-Author command

```
git commit -m "refactor: working db context and dependency injection





Co-authored-by: czambanini <ca.zambanini@gmail.com>
Co-authored-by: lauradefaria <lauradfma@gmail.com>
Co-authored-by: ig-nunes <inunes.us@gmail.com>
Co-authored-by: paulaandrezza <paulaandrezza25@gmail.com>
Co-authored-by: cristopherkovalski <cristopherkovalski@gmail.com>"
```

## ⚡ Desafio 1 - Inteligência Artificial
- No cenário empresarial, uma das tarefas que mais causam contratempos é o processo de lançamento de notas fiscais e cupons para reembolso. Muitas pessoas encontram dificuldades em preencher de forma correta, resultando em uma série de problemas, tanto para os colaboradores quanto para a equipe financeira da empresa, consumindo um tempo valioso de todos.

## 📋 Detalhe técnico
- Para resolver esse desafio, gostaríamos de utilizar o processamento de imagem para preenchermos os dados da despesa a partir do comprovante.

- Para extrairmos essas informações da despesa através de uma imagem, iremos utilizar a API de Imagem do ChatGPT-4 que será disponibilizada para vocês.

## 🚀 Proposta de solução
- Criar uma API de OCR para o lançamento de despesas através de uma imagem de comprovante.

- Essa API deverá enviar a imagem para o ChatGPT-4 e realizar alguns prompts para extrair as informações necessárias para o preenchimento.

- O primeiro passo seria validar o comprovante `prompt: essa imagem e algum comprovante de comprovante fiscal? responda com SIM ou NAO` Caso o comprovante seja inválido, devemos retornar a seguinte informação:

```
HTTP/1.1 400 Bad Request
{
  "message": "Comprovante Inválido"
}
```

- Em seguida, poderíamos extrair as informações da despesa.
  
  - Categoria da despesa `prompt: que categoria de despesa é essa, entre: hospedagem, transporte, viagem, alimentação ou Outros`. (especificar RESPONDA COM HOSPEDAGEM ou TRANSPORTE ou VIAGEM ou ALIMENTACAO ou OUTROS)
  - Valor total da despesa `prompt: qual o valor dessa despesa` (apenas números, exemplo)
  - Descricao da despesa `prompt: descreva sobre a despesa` (texto corrido especificamente, der um comando mais específico pra ter menos texto desnecessário) 
    
- Após extrair todas as informações, devemos gravar em um banco de dados qualquer a despesa na seguinte estrutura de exemplo:

```
{
  "Total": 99.00
  "Categoria": "HOSPEDAGEM",
  "Descricao": "Descricao do comprovante",
  "Status": "SUBMETIDO"
}
```

- Criar endpoint de consulta de despesas para que o Financeiro consiga visualizar todas as notas com status em SUBMETIDO.

- Criar endpoint para que o financeiro consiga mudar o status da despesa para PAGA.
