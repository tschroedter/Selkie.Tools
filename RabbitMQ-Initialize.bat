@ECHO OFF
ECHO RabbitMQ
ECHO Installing on Windows
ECHO https://www.rabbitmq.com/install-windows.html
ECHO -------------------------------------------------------
ECHO Installing on Windows (manual)
ECHO https://www.rabbitmq.com/install-windows-manual.html
ECHO -------------------------------------------------------
ECHO Install RabbitMQ under Windows
ECHO http://arcware.net/installing-rabbitmq-on-windows/
ECHO -------------------------------------------------------
ECHO Creating Selkie related vhost and users...
ECHO (run this script in C:\Program Files\RabbitMQ Server\rabbitmq_server-3.6.0\sbin)
call rabbitmqctl add_user selkie selkie
call rabbitmqctl add_user selkieAdmin selkieAdmin
call rabbitmqctl set_user_tags selkieAdmin administrator
call rabbitmqctl add_vhost selkie
call rabbitmqctl set_permissions -p selkie selkie ".*" ".*" ".*"
call rabbitmqctl set_permissions -p selkie selkieAdmin ".*" ".*" ".*"