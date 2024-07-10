echo 'This could be done a better way but for now sleeping 10 works'
sleep 10

curl -u elastic:$ELASTIC_PASSWORD \
-X POST \
http://elasticsearch:9200/_security/user/kibana_system/_password \
-d '{"password":"'"$KIBANA_PASSWORD"'"}' \
-H 'Content-Type: application/json'