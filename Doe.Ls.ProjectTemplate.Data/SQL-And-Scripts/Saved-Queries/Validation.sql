﻿--Validating Div=>DIR=>FA=>

SELECT  EXEC_CODE, FUNCTIONAL_AREA_CDE ,COUNT(DISTINCT DIRECTORATE_CDE) AS REPEATED
FROM            zzzzz_ED_SERVICES
GROUP BY EXEC_CODE, FUNCTIONAL_AREA_CDE
HAVING COUNT(DISTINCT DIRECTORATE_CDE) >1