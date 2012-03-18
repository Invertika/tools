rm -f /home/manaserv/data/
mkdir /home/manaserv/data/
cp ../../client-data/* /home/manaserv/data/
cp -R ../../client-data/maps/ /home/manaserv/data/
cp -R ../../server-data/scripts/ /home/manaserv/data/
find /home/manaserv/data/ -type d -exec chmod 777 {} +  
find /home/manaserv/data/ -type f -exec chmod 777 {} +  