FROM circleci/android:api-30
MAINTAINER Dara Keon
RUN sudo apt update

ENV JVM_OPTS=Xms256m\ -Xmx8192m
