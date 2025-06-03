"""Gunicorn *production* config file"""

from logging import getLogger, INFO, Formatter
from traceback import format_exc

from midna.cloudwatch import Boto3CloudWatchHandler


error_logger = getLogger("gunicorn.error")
error_logger.setLevel(INFO)

access_logger = getLogger("gunicorn.access")
access_logger.setLevel(INFO)

try:
    cloud_watch_handler = Boto3CloudWatchHandler()
    cloud_watch_handler.setFormatter(Formatter(cloud_watch_handler.FORMAT))
    error_logger.addHandler(cloud_watch_handler)
    access_logger.addHandler(cloud_watch_handler)
except Exception as e:
    error_logger.warning(f"CloudWatch handler failed: {e}")
    error_logger.warning(format_exc())


# Django WSGI application path in pattern MODULE_NAME:VARIABLE_NAME
wsgi_app = "midna.wsgi:application"

# The granularity of Error log outputs
loglevel = "debug"

# The number of worker processes for handling requests
import multiprocessing
workers = multiprocessing.cpu_count() * 2 + 1

# The socket to bind
bind = "0.0.0.0:8627"

# Restart workers when code changes (development only!)
reload = False

# Write access and error info to /var/log
accesslog = "-"
errorlog = "-"

# Redirect stdout/stderr to log file
capture_output = True

# PID file so you can easily fetch process ID
pidfile = "/var/run/gunicorn/prod.pid"

# Daemonize the Gunicorn process (detach & enter background)
daemon = False
