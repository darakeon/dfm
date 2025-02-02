export CIRCLE_BRANCH=$(git branch --show-current)

ORIGIN=$(git remote get-url origin)

ORIGIN_PARTS=$ORIGIN
# git@github.com:darakeon/dfm.git

ORIGIN_PARTS=(${ORIGIN_PARTS//:/ })
ORIGIN_PARTS=${ORIGIN_PARTS[1]}
# darakeon/dfm.git

ORIGIN_PARTS=(${ORIGIN_PARTS//./ })
ORIGIN_PARTS=${ORIGIN_PARTS[0]}
# darakeon/dfm

ORIGIN_PARTS=(${ORIGIN_PARTS//\// })

export CIRCLE_PROJECT_USERNAME=${ORIGIN_PARTS[0]}
export CIRCLE_PROJECT_REPONAME=${ORIGIN_PARTS[1]}
