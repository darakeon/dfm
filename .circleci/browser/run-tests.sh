DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

set -e

cd site/Tests/Browser/server

./DFM.MVC > ../../../../outputs/logs/site/Browser/server.log 2> ../../../../outputs/logs/site/Browser/error.log & disown

echo
echo "Calling site"
$DIR/call-site.sh
echo "Site awake!"

cd ../

echo
echo "Initializing DB data"
node helpers/setup.js

npm test $1
