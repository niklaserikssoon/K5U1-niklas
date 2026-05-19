# CloudNative Inventory API

## 1. Lösningsbeskrivning

Ett REST API byggt med ASP.NET Core 9 som hanterar ett produktlager (inventory). API:t exponerar endpoints för att lista produkter och verifiera att hemligheter laddas säkert via Azure Key Vault.

### Azure-tjänster som används

| Tjänst | Namn | Syfte |
|---|---|---|
| Azure Container Registry | `crcontainercloudnativeinventory` | Lagrar Docker-imagen |
| Azure Container Apps | `ca-inventory-api` | Kör API:t i molnet |
| Azure Key Vault | `kvnativeclaudeinventory` | Lagrar hemligheter säkert (t.ex. `ExternalServices:VendorApiKey`) |

---

## 2. Köra API:t lokalt

### Krav
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Docker Desktop (valfritt, för att köra som container)

### Utan Docker

```bash
cd CloudNativeInventory.Api
dotnet run
```

API:t är tillgängligt på `http://localhost:5000`.

### Hemligheter lokalt (utan att checka in dem)

Använd .NET User Secrets för att sätta `ExternalServices:VendorApiKey` lokalt:

```bash
cd CloudNativeInventory.Api
dotnet user-secrets init
dotnet user-secrets set "ExternalServices:VendorApiKey" "din-lokala-testnyckel"
```

Hemligheter sparas utanför repot och checkas aldrig in i Git.

### Med Docker

```bash
docker build -t inventory-api .
docker run -p 8080:8080 inventory-api
```

API:t är tillgängligt på `http://localhost:8080`.

---

## 3. CI/CD-pipeline

### Vad som triggar pipelinen

Pipelinen triggas automatiskt vid varje push till `master` eller om den heter `main` -branchen.

### Steg i pipelinen

När kod pushas till master triggas GitHub Actions-pipelinen automatiskt.

Pipelinen består av två steg: build-and-test och deploy.

I steget build-and-test checkas koden ut, .NET SDK installeras och projektets beroenden återställs. Därefter byggs applikationen och samtliga tester körs. Deploy-steget startar endast om detta steg lyckas.

I steget deploy checkas koden ut igen och pipelinen loggar in mot Azure Container Registry (ACR). En Docker-image byggs och pushas därefter till ACR. Efter inloggning mot Azure deployas den nya imagen till Container Apps-miljön.

Deploy och verifiering

Deploy sker automatiskt via GitHub Actions när kod pushas till master. Ingen manuell hantering krävs.

För att verifiera att API:t körs öppnas Application URL för Container Appen ca-inventory-api i Microsoft Azure Portal. Därefter kan relevanta endpoint-anrop användas för att kontrollera att deploymenten lyckats och att integrationer fungerar korrekt.

Deploy-jobbet kör **endast** om build-and-test lyckas (`needs: build-and-test`).

---

## 4. Deploy och verifiering

### Deploy

Deploy sker automatiskt via GitHub Actions när kod pushas till `master`. Ingen manuell åtgärd krävs.

### Verifiera att API:t körs

Öppna Container App-URL:en i Azure Portal (`ca-inventory-api` → Application URL) och anropa:

```
GET https://<din-container-app-url>/api/inventory
```

### Verifiera att hemligheter laddas säkert

```
GET https://<din-container-app-url>/api/inventory/system/verify-integration
```

**Förväntat svar när hemligheten är korrekt konfigurerad via Key Vault:**

```json
{
  "status": "Secured",
  "message": "Hemlighet laddades framgångsrikt via säker konfiguration."
}
```

**Svar om hemligheten saknas eller är den lokala dev-nyckeln:**

```json
{
  "status": "Unsecured",
  "message": "Körs med lokal (eller saknad) hemlighet!"
}
```

---

## 5. ADR – Architecture Decision Record

### ADR-001: Containerisering med Docker och deploy via Azure Container Apps

**Status:** Accepted

**Kontext:**
Applikationen behöver köras i molnet på ett skalbart och reproducerbart sätt. Lösningen ska vara enkel att deployas automatiskt via CI/CD och inte kräva hantering av underliggande servrar.

**Beslut:**
Applikationen paketeras som en Docker-container och deployas till Azure Container Apps. Docker-imagen lagras i Azure Container Registry (ACR).

**Motivering:**
- Container Apps är serverless — ingen infrastruktur att hantera
- ACR integreras nativt med Container Apps och GitHub Actions
- Docker garanterar att samma image körs lokalt som i produktion

**Konsekvenser:**
- Varje push till `master` resulterar i en ny Docker-image och automatisk deploy
- Lokal utveckling kräver Docker Desktop för att testa i container-miljö

---

### ADR-002: Hemlighetshantering via Azure Key Vault

**Status:** Accepted

**Kontext:**
API:t behöver tillgång till externa API-nycklar och andra känsliga konfigurationsvärden. Dessa får inte checkas in i Git eller hårdkodas i applikationen.

**Beslut:**
Hemligheter lagras i Azure Key Vault (`kvnativeclaudeinventory`) och laddas in i Container Apps som miljövariabler vid runtime. Lokalt används .NET User Secrets.

**Motivering:**
- Key Vault är Azures rekommenderade lösning för hemlighetshantering
- Hemligheter lämnar aldrig Git-historiken
- Endpoint `/api/inventory/system/verify-integration` verifierar att rätt hemlighet är laddad

**Konsekvenser:**
- Utvecklare måste sätta upp User Secrets lokalt för att testa `verify-integration`-endpointen
- CI/CD-pipelinen behöver rätt behörigheter mot Key Vault och ACR via en Service Principal
