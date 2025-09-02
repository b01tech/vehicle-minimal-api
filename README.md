# Vehicle Minimal API

Uma API RESTful para gerenciamento de veículos, desenvolvida com .NET 8 e Entity Framework Core.

## Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **Mysql**: Banco de dados
- **xUnit**: Framework de testes
- **JwtBearer**: Autenticação via token JWT

## Funcionalidades

- ✅ Listar veículos com paginação
- ✅ Buscar veículo por ID
- ✅ Cadastrar novo veículo
- ✅ Atualizar dados do veículo
- ✅ Excluir veículo (soft delete)
- ✅ Validação de placa duplicada
- ✅ Tratamento de erros e exceções

## Arquitetura

O projeto segue com as seguintes camadas:

- **Domain**: Entidades, interfaces e regras de negócio
- **Services**: Lógica de aplicação e mapeamento
- **Infrastructure**: Repositórios e acesso a dados
- **API**: Controllers e configurações

## Estrutura do Projeto

```
vehicle-minimal-api/
├── src/
│   └── VehicleControl.API/
│       ├── Domain/
│       │   ├── Entities/
│       │   └── Interfaces/
│       ├── Services/
│       │   ├── Entities/
│       │   └── Mapping/
│       ├── Infra/
│       │   ├── Data/
│       │   └── Repositories/
│       ├── DTOs/
│       ├── Extensions/
│       └── Exceptions/
└── test/
    └── VehicleControl.Test/

```

---

## Instalação e Configuração

### Pré-requisitos

- .NET 8 SDK
- MySQL (LocalDB ou instância completa)
- Visual Studio 2022 ou VS Code

### Passos para instalação

1. **Clone o repositório**

   ```bash
   git clone https://github.com/b01tech/vehicle-minimal-api.git
   cd vehicle-minimal-api
   ```

2. **Restaure as dependências**

   ```bash
   dotnet restore
   ```

3. **Configure as variáveis de ambiente**

   Edite o arquivo `appsettings.json` e configure as variáveis de ambiente:

   ```json
   {
      "ConnectionStrings": {
    "MySqlString": "Server=localhost;Database=VehicleControlDB;User=<your-user>;Password=<your-password>;"
      },
      "Settings": {
        "SecretKey": "<your-secret-to-hash-password>",
        "Jwt": {
          "Issuer": "VehicleControlAPI",
          "Audience": "VehicleControlClient",
          "SecretKey": "<your-secret-to-authentication>",
          "ExpirationMinutes": 60
        }
    }
   ```

4. **Execute a aplicação**
   ```bash
   dotnet run --project src/VehicleControl.API
   ```

A API estará disponível em `https://localhost:7000` ou `http://localhost:5000`.

### Banco de Dados

O projeto utiliza Entity Framework Core com MySQL.
As migrações são aplicadas automaticamente na inicialização da aplicação através de um método de extensão no `Program.cs`.

### Logging

O sistema de logging está configurado para diferentes níveis:

- **Development**: Information e acima
- **Production**: Warning e acima

---

## Uso da API

### Endpoints Disponíveis

#### Listar Veículos (com paginação)

```http
GET /vehicles/{page}
```

**Exemplo:** `GET /vehicles/1`

**Resposta:**

```json
{
  "vehicles": [
    {
      "id": 1,
      "active": true,
      "licencePlate": "ABC-1234",
      "model": "Honda Civic",
      "year": 2020
    }
  ],
  "currentPage": 1,
  "totalPages": 1,
  "totalItems": 1
}
```

#### Buscar Veículo por ID

```http
GET /vehicles/details/{id}
```

#### Cadastrar Veículo

```http
POST /vehicles
Content-Type: application/json

{
  "licencePlate": "ABC-1234",
  "model": "Honda Civic",
  "year": 2020
}
```

#### Atualizar Veículo

```http
PUT /vehicles/{id}
Content-Type: application/json

{
  "licencePlate": "XYZ-9876",
  "model": "Toyota Corolla",
  "year": 2021
}
```

#### Excluir Veículo

```http
DELETE /vehicles/{id}
```

### Códigos de Status

- `200 OK`: Operação realizada com sucesso
- `201 Created`: Recurso criado com sucesso
- `204 No Content`: Operação realizada sem conteúdo de retorno
- `400 Bad Request`: Dados inválidos
- `404 Not Found`: Recurso não encontrado
- `500 Internal Server Error`: Erro interno do servidor

---

## Testes

O projeto possui uma cobertura de testes:

### Tipos de Testes

- **Testes Unitários**: Services, Repositories, Mappers

### Executar os Testes

```bash
# Executar todos os testes
dotnet test
```

---

## Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
