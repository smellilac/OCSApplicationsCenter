-------------Работа с проектом: с помощью repl---------------------

1) В PowerShell в папке проекта dotnet run

2) В другом окне PowerShell dotnet tool install -g Microsoft.dotnet-httprepl
PS C:\Users\User> dotnet tool install -g Microsoft.dotnet-httprepl
httprepl http://localhost:"yourhost" - будет указан после dotnet run
Устанавливаем текстовый редактор для repl: pref set editor.command.default "C:\Program Files (x86)\Notepad++\notepad++.exe"
Теперь в этом окне можно PowerShell можно использовать команды
После post и put запросов открывается Ваш редактор кода, в нём будет предложено создать новый файл, создаём, вставляем json для команды, сохраняем файл и закрываем его


3) Список комманд, в том же порядке, что и на Вашем сайте:
post applications                                                   создание заявки


put applications/c4a0dc6b-3388-4cd6-b034-809802d86999               редактирование заявки


delete applications/c4a0dc6b-3388-4cd6-b034-809802d86999            удаление заявки


get applications/53e52dcd-0516-4240-8c8a-e67987e45396/submit        отправка заявки на рассмотрение программным комитетом


!!!!!Изменения при получения заявки по времени (между восклицательными знаками новые команды)!!!!


get applications?submittedAfter=2024-01-01T23:00:00.00              получение заявок поданных после указанной даты


get applications?unsubmittedOlder=2024-01-01T23:00:00.00            получение заявок не поданных и старше определенной даты


!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


get users/dddea753-d878-4bfe-a5d7-e9771e830cbd/currentapplication   получение текущей не поданной заявки для указанного пользователя


get applications/c4a0dc6b-3388-4cd6-b034-809802d86999               получение заявки по идентификатору


get activities                                                      получение списка возможных типов активности

4) Готовые JSON для post и put методов


"Report"    "Masterclass"   "Discussion"


Создание заявки: 
{
	"author": "ddfea953-d878-4bfe-a5d7-e9771e830cbd",
	"name": "YourText",
	"description": "YourText",
	"activity": "Masterclass", 
	"outline": "YourText"
}
