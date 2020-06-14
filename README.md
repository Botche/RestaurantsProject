# RestaurantsProject
Project for my graduation at SoftUni.

[The application is hosted in azure](https://restaurantsplatform.azurewebsites.net/ "Restaurants Platform")

Платформата има за цел да презентира различни видове ресторанти под категории.

Не логнатите потребители могат да разглеждат ресторантите, коментарите към тях.
За всеки ресторант можеш да видиш информация за него, къде се намира, галерия и коментари от посетили го вече потребители.

Има три роли:
 - User: Логнатите потребители могат да запазват ресторанти в любими, да ги коментират и да upvote и downvote коментарите към ресторантите. Имат си профил страница в коята могат да видят статистика за себе от вида на колко коментара са поставили, колко пъти са си давали гласът и кои са им любимите ресторанти.
 - Owner: Има същите права като User ролята, но може и да създава ресторанти.
 - Administrator: Има правата на всички роли. Също има права да създава категории ресторанти и да управлява потребителите (да има дава/взема права и да ги блокира). 


Users to test with 
 - admin@admin.admin - Password123
 - owner@owner.owner - Password123
 - user@user.user - Password123

TODO:
* Subcomments
* Add custom working time
* Elastic search