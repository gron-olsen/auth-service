export Secret="bb1b09b5f4c739f9956f59c7a1d2a67a7e09572bc1c6fd69ac5d17e8f8d1d33e1d4a738e6f8f5b1c977d6c5980a19a3b3c5f4f2b97e5c8cbd4a7d97f3c6e"
export Issuer="2D7412E88BAA48A69EA74D282ED7D"
export apiGetUser="localhost:5102/user/GetUser"
dotnet run Issuer="$Issuer" Secret="$Secret" apiGetUser="$apiGetUser" ValidAudience="http://localhost"