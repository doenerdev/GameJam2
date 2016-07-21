using UnityEngine;
using System.Collections;

public interface IValidConnectionTest
{

    bool IsValidConnection(Dot baseDot, Dot dot, Connection connection);
    bool IsDotOnOppositeSite(Dot baseDot, Dot dot);
    bool IsDotOnSameColumnOrRow(Dot baseDot , Dot dot);
    bool IsPathClear(Dot baseDot, Dot dot, Connection connection);

}
