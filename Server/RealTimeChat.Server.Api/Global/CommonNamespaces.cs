/****************************************************************************************************************************/
// GlobalNamespaces sınıfı dotnet'in sunduğu namespace'lerin artık sınıfların üzerinde yazılma zorunluluğunun ortadan       //
// kalkması ile kullandığımız bir yapıdır. Bu sınıf içerisine başında `global` etiketi ile eklediğimiz tüm namespace'ler    //
// artık aynı assembly içerisindeki diğer sınıflar içinde eklenmiş olacaktır. Dolayısıyla aynı namespace'i tekrar tekrar    //
// eklememize gerek yoktur. Burada eklenen her namespace için bir alan yaratıp onun altına ilgili namespace'leri eklemek    //
// bize aradığımızda daha kolay bir ulaşım sağlayacaktır.                                                                   //
//                                                                                                                          //
// ** NOT ** : EKLENEN TÜM LIBRARY'LER AİT OLDUĞU ALANIN ALTINA VE ALFABETİK SIRALAMA BAZ ALINARAK EKLENMELİDİR.            //
/****************************************************************************************************************************/

/* (PROJECT) */

global using RealTimeChat.Server.Api.Hubs;

/* (NUGET) */

global using Microsoft.AspNetCore.SignalR;