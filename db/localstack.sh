#!/bin/sh
echo "Initializing localstack s3"

echo $LOGS_GROUP
echo $LOGS_REGION
awslocal logs create-log-group --log-group-name $LOGS_GROUP --region $LOGS_REGION
