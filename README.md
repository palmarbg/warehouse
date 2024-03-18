Hagyj itt valami szöveget, Vándor!


* Ismert bugokat vagy javítani való dolgokat pakoljuk be issuekhoz pls

* Magic számokat nevezzük el

* Ha a függvény neve nem egyértelmű dobjunk elé egy "dokumentációs" kommentet

* `Map` indexelése: `Map[x,y]`, X-tengely balról jobbra, Y-tengely fentről lefelé

## Branchek
Kb ez alapján gondoltam
[link](https://nvie.com/posts/a-successful-git-branching-model/),
hogy van 2 fő branch:

* develop - amikor egy feature kész van ebbe mergeljünk
* master - ha valami komplett demo kész van akkor a develop ágat mergeljük a masterbe
* feature branchek - ha valami featuret elkezdesz, cloneozd a develop branchet, majd ha kész vagy akkor a branchet mergeld a developba vissza
* bugfix branch - (?)
