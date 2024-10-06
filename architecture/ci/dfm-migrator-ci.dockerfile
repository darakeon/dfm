FROM darakeon/dfm-migrator
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

RUN apk add git
RUN apk add openssh
RUN apk add github-cli

COPY architecture/ci/config-git /usr/bin
RUN chmod +x /usr/bin/config-git

COPY architecture/ci/merge-by-branch /usr/bin
RUN chmod +x /usr/bin/merge-by-branch

RUN pip install boto3 --break-system-packages
