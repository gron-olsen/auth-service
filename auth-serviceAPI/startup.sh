export Secret="RasmusGrønErSuperCoolOgDenBedsteChef!"
export Issuer="Gron&OlsenGruppen"
export Valid="http://localhost"
export apiGetUser="localhost:5102/user/GetUser"
dotnet run Secret="$Secret" Issuer="$Issuer" apiGetUser="$apiGetUser" Valid="$Valid"
#chmod +x ./startup.sh
#./startup.sh
#Login -> receive token -> input token at https://jwt.io -> Authorize through Bearer Token