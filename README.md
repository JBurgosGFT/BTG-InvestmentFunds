# InvestmentFunds.Api

API para la gestión de fondos de inversión, clientes, suscripciones, transacciones y notificaciones, utilizando .NET 8, Arquitectura Limpia (Clean Code), DDD (Domain-Driven Design) Patrones de diseño (Factory, Strategy, CQRS) con MediatR y Servicios de AWS (DynamoDB, Cognito, SecretManager).

-Patron de diseño CQRS para la separacion de responsabilidades de lectura y escritura (Command, Querys, Handlers).
-Patron de diseño Factory para la definición de interfaces y expocisión de funcionalidad con implementacion de los contratos definidos (Repositorios).
-Patron de diseño Strategy para definir un algoritmo, encapsularlos y hacerlo intercambiable segun la necesidad (Notificacion: Email, SMS).
-Arquitectura DDD para basarse en las reglas del negocio como funcion principal del proyecto (Entidades, Enum).
-Arquitectura limpia para la separacion de responsabilidad, mantenimiento de codigo y escalabilidad del proyecto (Presentacion, Aplicacion, Infrastructura, Dominio).

---

## Características principales

- **Clientes:** Registro, consulta y actualización de clientes.
- **Fondos:** Consulta de fondos de inversión y sus características.
- **Suscripciones:** Suscripción y cancelación a fondos, con validación de saldo y reglas de negocio.
- **Transacciones:** Registro y consulta de movimientos (apertura, cancelación) por cliente y por rango de fechas.
- **Notificaciones:** Envío de notificaciones por Email o SMS según preferencia del cliente.
- **Autenticación y autorización:** Integración con AWS Cognito y perfilamiento por roles.
- **Persistencia:** Modelos NoSQL en DynamoDB con índices secundarios para consultas eficientes.

---

## Estructura del proyecto

- **InvestmentFunds.Api:** Controladores y configuración principal de la API.
- **InvestmentFunds.Application:** Lógica de negocio, comandos, queries y handlers (MediatR).
- **InvestmentFunds.Domain:** Entidades y enums del dominio.
- **InvestmentFunds.Infrastructure:** Repositorios DynamoDB, mapeadores y estrategias de notificación.

---

## Modelos DynamoDB

- **CustomerDynamoModel:** Clientes, con atributos como saldo y preferencia de notificación.
- **FundDynamoModel:** Fondos de inversión, con categoría y monto mínimo.
- **SubscriptionDynamoModel:** Suscripciones, con índices por cliente y fondo.
- **TransactionDynamoModel:** Transacciones, con índices por cliente y fondo.

---

## Seguridad

- **Autenticación:** JWT Bearer con AWS Cognito.
- **Autorización:** Por roles (ej. Admin, Manager) en los endpoints.
- **Encriptación:** HTTPS para datos en tránsito.

---

## Pruebas unitarias

El proyecto incluye pruebas unitarias para todos los handlers principales (`QueryHandler` y `CommandHandler`) usando **xUnit** y **Moq**.  
Ejemplos de pruebas:
- Validación de reglas de negocio.
- Verificación de llamadas a repositorios y mediadores.
- Comprobación de respuestas y excepciones.

---

## Ejemplo de uso de la API

### Suscribir a un fondo

```http
POST /api/subscriptions/subscribe
{
  "fundId": 1,
  "customerId": "GUID"
}
```

### Cancelar suscripción

```http
GET /api/subscriptions/unsubscribe/{subscriptionId}
```

### Consultar transacciones por cliente

```http
GET /api/transactions?customerId=GUID&type=Opening
```

---

## Requisitos

- .NET 8+
- AWS DynamoDB
- AWS Cognito
- AWS Secret Manager
- Visual Studio Code o Visual Studio

---

## Ejecución local

1. Configura las credenciales de AWS y los nombres de tablas en `appsettings.json`.
2. Ejecuta la API:
   ```bash
   dotnet run --project InvestmentFunds.Api
   ```
3. Accede a Swagger en `https://localhost:5001/swagger` para probar los endpoints.