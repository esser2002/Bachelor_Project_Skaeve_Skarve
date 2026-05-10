set D=10
set k=%1
set m=%2
set epsilon=0.11
set minPts=50
set pathtoout=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out
set inputdata=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\mnist_test.csv
set clusteroutput=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\mnist_test_sDBSCAN_Clusters.csv
set statsoutput=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\mnist_test_statsoutput.csv
set normalisedDataRaw=\normalisedDataMnistTest.csv


cd ..\sDBSCAN\
dotnet run "%inputdata%" "%clusteroutput%" "%statsoutput%" "%D%" "%k%" "%m%" "%epsilon%" "%minPts%"
cd .\python\
echo %pathtoout% %statsoutput% %normalisedDataRaw% %clusteroutput% %epsilon% %minPts% | uv run nmi.py
cd ..
