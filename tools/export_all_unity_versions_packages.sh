#bash
source .setup_env_vars.sh

echo "export package for Unity 5.6"
cd $PROJ_5_6_PATH
sh .defaultExport.sh

echo "export package for Unity 2017"
cd $PROJ_2017_PATH
sh .defaultExport.sh

echo "export package for Unity 2018"
cd $PROJ_2018_PATH
sh .defaultExport.sh