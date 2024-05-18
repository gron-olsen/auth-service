
export Secret="VeryLongAndVerySecretSecretThatIsSuperSecretive"
export Issuer="VeryLongAndVerySecretIssuerThatIsSuperGood"
export apiGetUser="localhost:5102/user/GetUser"
dotnet run Issuer="$Issuer" Secret="$Secret" apiGetUser="$apiGetUser"

# # Command som giver execute permission: chmod +x ./startup.sh # # 