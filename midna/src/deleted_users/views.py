from django.contrib.auth.decorators import login_required
from django.http import JsonResponse

from .models import Wipe
from utils.s3 import S3


