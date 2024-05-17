export Secret="SecretSecret"
export Issuer="IssuerIssuer"
export apiGetUser="localhost:5102/user/GetUser"
dotnet run Issuer="$Issuer" Secret="$Secret" apiGetUser="$apiGetUser" ValidAudience="http://localhost"