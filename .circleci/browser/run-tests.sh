DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

set -e

cd site/Tests/Browser/server

./DFM.MVC p2709 > ../log/server.log & disown

echo
echo "Calling site"
$DIR/call-site.sh
echo "Site awake!"

cd ../

echo
echo "Initializing DB data"
node contract.js 2> /dev/null

npm test -- --runInBand
