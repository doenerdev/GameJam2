using UnityEngine;
using System.Collections;

public enum WalkDirection
{
    horzPos,
    horzNeg,
    vertPos,
    vertNeg,
    none,
}

public static class CubeLookup  {

    public static WalkDirection GetWalkDirection(CubeSide baseSide, CubeSide destSide, Dot baseDot, Dot destDot)
    {
        Debug.Log(baseSide + "  " + destSide);

        if (baseSide == destSide)
        {
            if ((int) baseDot.ParentSubGrid.SubGridPosition.x == (int) destDot.ParentSubGrid.SubGridPosition.x)
                return (baseDot.ParentSubGrid.SubGridPosition.y > destDot.ParentSubGrid.SubGridPosition.y)
                    ? WalkDirection.vertNeg
                    : WalkDirection.vertPos;
            else if((int)baseDot.ParentSubGrid.SubGridPosition.y == (int)destDot.ParentSubGrid.SubGridPosition.y)
                return (baseDot.ParentSubGrid.SubGridPosition.y > destDot.ParentSubGrid.SubGridPosition.y)
                    ? WalkDirection.horzNeg
                    : WalkDirection.horzPos;
        }

        switch (baseSide)
        {
            case CubeSide.front:
                if (destSide == CubeSide.top)
                    return WalkDirection.vertPos;
                else if (destSide == CubeSide.bottom)
                    return WalkDirection.vertNeg;
                else if (destSide == CubeSide.left)
                    return WalkDirection.horzNeg;
                else if (destSide == CubeSide.right)
                    return WalkDirection.horzPos;
                break;
            case CubeSide.back:
                if (destSide == CubeSide.top)
                    return WalkDirection.vertPos;
                else if (destSide == CubeSide.bottom)
                    return WalkDirection.vertNeg;
                else if (destSide == CubeSide.left)
                    return WalkDirection.horzPos;
                else if (destSide == CubeSide.right)
                    return WalkDirection.horzNeg;
                break;
            case CubeSide.top:
                if (destSide == CubeSide.front)
                    return WalkDirection.vertNeg;
                else if (destSide == CubeSide.back)
                    return WalkDirection.vertPos;
                else if (destSide == CubeSide.left)
                    return WalkDirection.horzNeg;
                else if (destSide == CubeSide.right)
                    return WalkDirection.horzPos;
                break;
            case CubeSide.bottom:
                if (destSide == CubeSide.front)
                    return WalkDirection.vertPos;
                else if (destSide == CubeSide.back)
                    return WalkDirection.vertNeg;
                else if (destSide == CubeSide.left)
                    return WalkDirection.horzNeg;
                else if (destSide == CubeSide.right)
                    return WalkDirection.horzPos;
                break;
            case CubeSide.right:
                if (destSide == CubeSide.top)
                    return WalkDirection.vertPos;
                else if (destSide == CubeSide.bottom)
                    return WalkDirection.vertNeg;
                else if (destSide == CubeSide.front)
                    return WalkDirection.horzNeg;
                else if (destSide == CubeSide.back)
                    return WalkDirection.horzPos;
                break;
            case CubeSide.left:
                if (destSide == CubeSide.top)
                    return WalkDirection.vertPos;
                else if (destSide == CubeSide.bottom)
                    return WalkDirection.vertNeg;
                else if (destSide == CubeSide.front)
                    return WalkDirection.horzPos;
                else if (destSide == CubeSide.back)
                    return WalkDirection.horzNeg;
                break;
        }
        return WalkDirection.none;
    }

    public static ConnectionDirectionGeneral GetDirectionForSecondSideVert(CubeSide baseSide)
    {
        switch (baseSide)
        {
            case CubeSide.right:
            case CubeSide.left:
                return ConnectionDirectionGeneral.horizontal;
                break;
            default:
                return ConnectionDirectionGeneral.vertical;
        }
    }

    public static ConnectionDirectionGeneral GetDirectionForSecondSideHorz(CubeSide baseSide)
    {
        switch (baseSide)
        {
            case CubeSide.top:
            case CubeSide.bottom:
                return ConnectionDirectionGeneral.vertical;
                break;
            default:
                return ConnectionDirectionGeneral.horizontal;
        }
    }

    public static int GetVertStartIndex(CubeSide baseSide, CubeSide destSide, Dot baseDot)
    {
        switch (baseSide)
        {
            case CubeSide.front:
                if (destSide == CubeSide.top)
                    return 0;
                else
                    return 2;
                break;
            case CubeSide.back:
                if (destSide == CubeSide.top)
                    return 2;
                else
                    return 0;
                break;
            case CubeSide.top:
                if (destSide == CubeSide.back)
                    return 2;
                else
                    return 0;
                break;
            case CubeSide.bottom:
                if (destSide == CubeSide.back)
                    return 0;
                else
                    return 2;
                break;
            case CubeSide.right:
                if (destSide == CubeSide.top)
                    return (int)baseDot.ParentSubGrid.SubGridPosition.x;
                else
                    return 2 - (int)baseDot.ParentSubGrid.SubGridPosition.x;
                break;
            case CubeSide.left:
                if (destSide == CubeSide.top)
                    return 2 - (int)baseDot.ParentSubGrid.SubGridPosition.x;
                else
                    return (int)baseDot.ParentSubGrid.SubGridPosition.x;
                break;
            default:
                return 0;
        }
    }

    public static int GetHorzStartIndex(CubeSide baseSide, CubeSide destSide, Dot baseDot)
    {
        switch (baseSide)
        {
            case CubeSide.front:
                if (destSide == CubeSide.top)
                    return 0;
                else
                    return 2;
                break;
            case CubeSide.back:
                if (destSide == CubeSide.top)
                    return 2;
                else
                    return 0;
                break;
            case CubeSide.top:
                if (destSide == CubeSide.back)
                    return 2;
                else
                    return 0;
                break;
            case CubeSide.bottom:
                if (destSide == CubeSide.back)
                    return 0;
                else
                    return 2;
                break;
            case CubeSide.right:
                if (destSide == CubeSide.top)
                    return (int)baseDot.ParentSubGrid.SubGridPosition.x;
                else
                    return 2 - (int)baseDot.ParentSubGrid.SubGridPosition.x;
                break;
            case CubeSide.left:
                if (destSide == CubeSide.top)
                    return 2 - (int)baseDot.ParentSubGrid.SubGridPosition.x;
                else
                    return (int)baseDot.ParentSubGrid.SubGridPosition.x;
                break;
            default:
                return 0;
        }
    }

}
