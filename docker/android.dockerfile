FROM circleci/android:api-30
MAINTAINER Dara Keon
ENV JVM_OPTS=Xms8G\ -Xmx8G
RUN sudo apt update
