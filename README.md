# AuthService ![.NET](https://img.shields.io/badge/.NET-9.0-blue)

AuthService är en mikrotjänst som ansvarar för att hantera autentisering och auktorisering i ett distribuerat system.  
Den används av andra tjänster för att verifiera användares identitet och behörighet.  
AuthService är beroende av en extern tjänst, **TokenService**, för att generera och validera JWT-tokens.

---

## Funktionalitet

- Hanterar användarregistrering (signup) och inloggning (signin)
- Validerar autentiseringstokens hämtade från **TokenService**
- Säkerhetsfunktioner för att skydda känsliga data
- Integrerar med **TokenService** för token-hantering

---

## Endpoints

### `POST /auth/signup`

Registrerar en ny användare.

#### Request body:
```json
{
  "email": "exampleUser",
  "password": "examplePassword",
  "confirmPassword": "examplePassword",
}
```

#### Response:
```json
{
  "succeeded": true,
  "errors": [],
  "message": "User created successfully."
}
```

### `POST /auth/signin`

Verifierar användarens uppgifter och hämtar en autentiseringstoken från **TokenService**.

#### Request body:
```json
{
  "email": "exampleUser",
  "password": "examplePassword"
  "rememberMe": true
}
```

#### Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
  "email": "admin@ventixe.com",
  "name": "admin@ventixe.com",
  "role": "Admin"
}
```

### `GET /auth/userinfo`

Hämtar information om den autentiserade användaren.

#### Header:
```makefile
Authorization: Bearer <din-token>
```

#### Response:
```json
{
  "userId": "12345",
  "username": "exampleUser",
  "email": "user@example.com",
  "roles": ["admin", "member"]
}
```

---

## Kom igång lokalt

1. Klona repositoryt:
```bash
git clone https://github.com/Grupp-4-Ventixe/AuthServiceProvider.git
```
2. Öppna projektet i Visual Studio
3. Kör projektet med `Ctrl + F5`
→ Tjänsten startas på en lokal server (`https://localhost:xxxx/`)

4. Testa endpoints med valfritt API-verktyg (t.ex. Postman eller curl)

---

## Integration med TokenService

AuthService använder **TokenService** för att hantera JWT-tokens.  
TokenService är en separat mikrotjänst som utvecklats av Alec. Integration sker via följande steg:

1. Vid inloggning (`/auth/signin`) skickar AuthService användarens ID och roll till **TokenService**.
2. **TokenService** genererar och returnerar en signerad JWT-token.
3. AuthService returnerar token till klienten.

---

## Teknologi och beroenden

- ASP.NET Core Web API (.NET 9)
- `System.IdentityModel.Tokens.Jwt`
- `Microsoft.AspNetCore.Authentication`
- Extern tjänst: **TokenService**

---

## Publicering

AuthService är publicerad på Azure:

🔗 https://authservice-ventixe-fagve2emhbdnfpcn.swedencentral-01.azurewebsites.net/

---

## Författare

Utvecklad av Nino Hägglund
