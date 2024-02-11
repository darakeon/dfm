set -e

if [ "$1" == ".........." ]
	then
		echo "too much tries"
		exit 1
fi

echo $1

sleep 1
curl -s http://localhost:2703 > /dev/null || $0 $1.
