# Running tests

The tests are easier to run using docker.

This make is prepared to create the machine AND to be used inside of it.

At your terminal:

- `machine`: start the docker container to run tests
- `rm`: remove the docker container

Inside the docker machine:

- `setup`: build and run the site
- `clear`: stop site and clear db
- `resetup`: call clear then setup
- `test`: run the automated browser tests
