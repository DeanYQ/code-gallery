# php

## datetime
Use strtotime function of PHP.

```php
  echo strtotime('2012-03-27 18:47:00');

  $long = strtotime('2012-03-27 18:47:00'); //--> which results to 1332866820
  echo date('Y-m-d H:i:s', $long);

```

[stackoverflow link](http://stackoverflow.com/questions/21796958/how-to-convert-date-into-integer-number-in-php)  
[php datetime link](http://php.net/strtotime)