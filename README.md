Instrucciones generales para ejecutar el programa.
1. Dirigirse a la ruta donde está localizado el proyecto, en mí caso el path es:"C:\Users\j2005\OneDrive\Documentos\GitHub\Csharp-Whatsapp-Like\Whatsapp Like\
2. Una vez localizado en la ruta específicada en cada dispositivo nos dirigímos a la terminal/Powershell del entorno o Command Prompt y digitamos dotnet run --project "ruta" server 5000
esto para iniciar el servidor en el puerto 5000
3. Luego de ejecutar el servidor abrimos una nueva terminal destinado para el cliente e ingresamos el comando dotnet run --project "C:\Users\j2005\OneDrive\Documentos\GitHub\Csharp-Whatsapp-Like\Whatsapp Like\Whatsapp Like.csproj" client 5000
Luego de ello específicamos el puerto de destino y luego escribimos el mensaje que queramos mandar al servidor
4. Luego de enviar el mensaje el mensaje estará reflejado en el Command Prompt o Terminal del Servidor (Sí queremos varios clientes seguimos el paso tres con una nueva terminal, con ello, podemos tener varios clientes a la vez).
