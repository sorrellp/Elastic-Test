# Notes
To run a docker container with just bash the following can be used

```docker run -it {image} bash```

The -it parameter causes this to be a interactive teminal session within the container. Optionally, the -rm parameter to be added to have the container automatically cleaned up after exitting form it.

To test container setup this command can be used

```docker run --rm -it $(docker build -q .)```

This will build a dockerfile in the current directory and then automatically run it with an interactive session.