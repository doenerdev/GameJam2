using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class ValidConnectionBasic : IValidConnectionTest
{
    public bool IsValidConnection(Dot baseDot, Dot dot, Connection connection)
    {
        if (dot.DotState != DotState.active && dot.DotState != DotState.connected)
        {
            if (IsDotOnOppositeSite(baseDot, dot) == false)
            {
                if (IsDotOnSameColumnOrRow(baseDot, dot))
                {
                    return IsPathClear(baseDot, dot, connection);
                }
                else
                {
                    Debug.LogWarning("No valid connection. Dot is not on same row or column!");
                    return false;
                }
            }
            else
            {
                Debug.LogWarning("No valid connection. Dot is on opposite site!");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("No valid connection. Dot is already active!");
            return false;
        }
    }

    public bool IsDotOnOppositeSite(Dot baseDot, Dot dot)
    {
        return (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top &&
         baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.bottom) ||
        (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left &&
         baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right) ||
        (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.front &&
         baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back);
    }

    public bool IsDotOnSameColumnOrRow(Dot baseDot, Dot dot)
    {
        var baseSide = baseDot.ParentSubGrid.ParentGrid.CubeSide;
        var destSide = dot.ParentSubGrid.ParentGrid.CubeSide;

        Debug.Log(baseSide + "->" + destSide);

        if ((baseSide == CubeSide.top) && (destSide == CubeSide.right))
        {
            Debug.Log("valid:" + ((int)dot.SubGridPosition.x == (int)baseDot.SubGridPosition.y));
            return (int)dot.SubGridPosition.x == (int)baseDot.SubGridPosition.y;
        }
        else if ((baseSide == CubeSide.right) && destSide == CubeSide.top)
        {
            Debug.Log("valid:" + ((int)dot.SubGridPosition.y == (int)baseDot.SubGridPosition.x));
            return (int)dot.SubGridPosition.y == (int)baseDot.SubGridPosition.x;
        }
        else if ((baseSide == CubeSide.bottom) && (destSide == CubeSide.right))
        {
            Debug.Log("valid:" + ((int)dot.SubGridPosition.x == (int)baseDot.SubGridPosition.y));
            var baseX = ((int)baseDot.SubGridPosition.x == 0) ? 2 : ((int)baseDot.SubGridPosition.x == 2) ? 0 : 1;
            return (int)dot.SubGridPosition.y == baseX;
        }
        else if ((baseSide == CubeSide.right) && (destSide == CubeSide.bottom))
        {
            var destY = ((int)dot.SubGridPosition.y == 0) ? 2 : ((int)dot.SubGridPosition.y == 2) ? 0 : 1;
            Debug.Log("valid:" + (destY == (int)baseDot.SubGridPosition.x));
            return destY == (int)baseDot.SubGridPosition.x;
        }
        else if ((baseSide == CubeSide.top) && (destSide == CubeSide.left))
        {
            var destX = ((int)dot.SubGridPosition.x == 0) ? 2 : ((int)dot.SubGridPosition.x == 2) ? 0 : 1;
            return destX == (int)baseDot.SubGridPosition.y;
        }
        else if ((baseSide == CubeSide.left) && (destSide == CubeSide.top))
        {
            var baseX = ((int)baseDot.SubGridPosition.x == 0) ? 2 : ((int)baseDot.SubGridPosition.x == 2) ? 0 : 1;
            return baseX == (int)dot.SubGridPosition.y;
        }
        else if ((baseSide == CubeSide.left) && (destSide == CubeSide.bottom))
        {
            return (int)dot.SubGridPosition.y == (int)baseDot.SubGridPosition.x; // is this right or should it be switched?
        }
        else if ((baseSide == CubeSide.bottom) && (destSide == CubeSide.left))
        {
            return (int)dot.SubGridPosition.x == (int)baseDot.SubGridPosition.y; // is this right or should it be switched?
        }
        else if ((baseSide == CubeSide.bottom) && (destSide == CubeSide.back))
        {
            var baseX = ((int)baseDot.SubGridPosition.x == 0) ? 2 : ((int)baseDot.SubGridPosition.x == 2) ? 0 : 1;
            return baseX == (int)dot.SubGridPosition.x;
        }
        else if ((baseSide == CubeSide.back) && (destSide == CubeSide.bottom))
        {
            var destX = ((int)dot.SubGridPosition.x == 0) ? 2 : ((int)dot.SubGridPosition.x == 2) ? 0 : 1;
            return destX == (int)baseDot.SubGridPosition.x;
        }
        else if (((baseSide == CubeSide.back) && (destSide == CubeSide.top)) || ((baseSide == CubeSide.top) && (destSide == CubeSide.back)))
        {
            var baseX = ((int)baseDot.SubGridPosition.x == 0) ? 2 : ((int)baseDot.SubGridPosition.x == 2) ? 0 : 1;
            return baseX == (int)dot.SubGridPosition.x;
        }

        Debug.Log("kjfdjdskjkjdokn");

        return (int)dot.SubGridPosition.x == (int)baseDot.SubGridPosition.x || (int)dot.SubGridPosition.y == (int)baseDot.SubGridPosition.y;
    }

    public bool IsPathClear(Dot baseDot, Dot dot, Connection connection)
    {
        ConnectionDirectionGeneral generalDirection = Grid.FromSideToSideDirectionGeneral(baseDot.ParentSubGrid.ParentGrid.CubeSide,
                dot.ParentSubGrid.ParentGrid.CubeSide, baseDot, dot);

        if (generalDirection == ConnectionDirectionGeneral.horizontal)
            return IsPathClearHorizontal(baseDot, dot, connection);
        else
            return IsPathClearVertical(baseDot, dot, connection);

    }

    private bool IsPathClearVertical(Dot baseDot, Dot dot, Connection connection)
    {
        Debug.Log("vertical");
        var direction = Grid.FromSideToSideDirectionVertical(baseDot.ParentSubGrid.ParentGrid.CubeSide,
            dot.ParentSubGrid.ParentGrid.CubeSide);
        var end = (direction > 0) ? (int)GridManager.instance.GridSize : 0;



        if (dot.ParentSubGrid.ParentGrid.CubeSide == baseDot.ParentSubGrid.ParentGrid.CubeSide)
        {
            Debug.Log((int)baseDot.SubGridPosition.y + 1 + "  " + (int)dot.SubGridPosition.y);
            var loopOperator = (dot.SubGridPosition.y > baseDot.SubGridPosition.y) ? 1 : -1;

            if (loopOperator > 0)
                connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineVertPosGo, baseDot.ParentSubGrid);
            else
                connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineVetNegGo, baseDot.ParentSubGrid);

            for (int i = (int)baseDot.SubGridPosition.y + 1 * loopOperator; i != (int)dot.SubGridPosition.y; i = i + 1 * loopOperator)
            {
                if (i < 0 || i >= GridManager.instance.GridSize - 1)
                {
                    if (i < 0 || i > GridManager.instance.GridSize - 1) continue;
                }

                if (i >= 0 && i < GridManager.instance.GridSize)
                {
                    if (loopOperator > 0)
                        connection.AddPartToConnection(baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i).ConnectionLines.connectionLineVertPosGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                    else
                        connection.AddPartToConnection(baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i).ConnectionLines.connectionLineVetNegGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                }


                if (!baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i).IsPaintable)
                {
                    Debug.LogWarning("No valid connection. Path isn't clear!");
                    return false;
                }
            }
        }
        else
        {
            Debug.Log("vertdsg" + "  " + direction);
            Debug.Log((int)baseDot.SubGridPosition.y + 1 * direction + " " + end + " " + direction);

            if (!(dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top))
            {
                if (direction > 0)
                    connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineVertPosGo, baseDot.ParentSubGrid);
                else
                    connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineVetNegGo, baseDot.ParentSubGrid);
            }

            if ((int)baseDot.SubGridPosition.y + 1 * direction == end && (end >= 0 && end < GridManager.instance.GridSize))
            {
               if (direction > 0)
                    connection.AddPartToConnection(
                        baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, end)
                            .ConnectionLines.connectionLineVertPosGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, end));
                else
                    connection.AddPartToConnection(
                        baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, end)
                            .ConnectionLines.connectionLineVetNegGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, end));
            }


            if (!(direction > 0 && baseDot.SubGridPosition.y >= GridManager.instance.GridSize - 1) &&
                !(direction < 0 && (int)baseDot.SubGridPosition.y == 0))
            {
                for (int i = (int)baseDot.SubGridPosition.y + 1 * direction; i != end; i = i + 1 * direction)
                {

                    if (i >= 0 && i < GridManager.instance.GridSize)
                    {
                       if (direction > 0)
                            connection.AddPartToConnection(
                                baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, i)
                                    .ConnectionLines.connectionLineVertPosGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, i));
                        else
                            connection.AddPartToConnection(
                                baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, i)
                                    .ConnectionLines.connectionLineVetNegGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt((int)baseDot.SubGridPosition.x, i));
                    }

                    if (i < 0 || i > GridManager.instance.GridSize - 1 ||
                        i == (int)baseDot.ParentSubGrid.SubGridPosition.y)
                        continue;
                    Debug.Log(i);

                    var posX = (int)dot.SubGridPosition.x;
                    if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back &&
                         dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top))
                    {
                        posX = (posX == 0) ? 2 : (posX == 2) ? 0 : 1;
                    }

                    if (!baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(posX, i).IsPaintable)
                    {
                        Debug.LogWarning("No valid connection. Path isn't clear!");
                        return false;
                    }

                }
            }

            if (CubeLookup.GetDirectionForSecondSideVert(baseDot.ParentSubGrid.ParentGrid.CubeSide) ==
                ConnectionDirectionGeneral.vertical)
            {

                Debug.Log((Mathf.Abs(end - GridManager.instance.GridSize)) + "  " + (int)dot.SubGridPosition.y + "  " + (int)dot.SubGridPosition.x + " " + direction);

                var start = Mathf.Abs(end - GridManager.instance.GridSize);

                if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.bottom &&
                    dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back) || (baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back &&
                    dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.bottom))
                {
                    direction *= -1;
                    start = 0;
                }
                else if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top &&
                    dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back) || (baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back &&
                    dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top))
                {
                    direction *= -1;
                    start = GridManager.instance.GridSize;
                }

                //--------------------CORNER PIECE--------------------//
                if (!(dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.bottom || (baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top && dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.front)))
                {
                    if (direction > 0)
                        connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, 0).ConnectionLines.connectionLineVetNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, 0));
                    else
                        connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, GridManager.instance.GridSize - 1).ConnectionLines.connectionLineVertPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, GridManager.instance.GridSize - 1));
                }
                

                for (int i = start;
                    i != (int)dot.SubGridPosition.y;
                    i = i + 1 * direction)
                {
                    if (i >= 0 && i < GridManager.instance.GridSize)
                    {
                        if (!(baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.bottom))
                        {
                            if (direction > 0)
                                connection.AddPartToConnection(
                                    dot.ParentSubGrid.ParentGrid.GetSubGridAt((int) dot.SubGridPosition.x, i)
                                        .ConnectionLines.connectionLineVertPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                            else
                                connection.AddPartToConnection(
                                    dot.ParentSubGrid.ParentGrid.GetSubGridAt((int) dot.SubGridPosition.x, i)
                                        .ConnectionLines.connectionLineVetNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                        }
                        else
                        {
                            if (direction > 0)
                                connection.AddPartToConnection(
                                    dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i)
                                        .ConnectionLines.connectionLineVertPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                            else
                                connection.AddPartToConnection(
                                    dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i)
                                        .ConnectionLines.connectionLineVetNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                        }
                    }

                    if (i < 0 || i > GridManager.instance.GridSize - 1 || (int)dot.SubGridPosition.y == i)
                        continue;

                    if (!dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i).IsPaintable)
                    {
                        Debug.LogWarning("No valid connection. Path isn't clear!");
                        return false;
                    }
                }
            }
            else
            {
                var start = 0;
                if (baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right)
                    start = GridManager.instance.GridSize;

                if (start > 0) direction = -1;
                else direction = 1;
                Debug.Log(start + " " + (int)dot.SubGridPosition.x + "   " + direction);

                //--------------------CORNER PIECE--------------------//
                if (direction > 0)
                {
                    connection.AddPartToConnection(
                        dot.ParentSubGrid.ParentGrid.GetSubGridAt(0,
                            (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(0,
                            (int)dot.SubGridPosition.y));
                }
                else
                {
                    connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - 1, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - 1, (int)dot.SubGridPosition.y));
                }

                for (int i = start;
                    i != (int)dot.SubGridPosition.x;
                    i = i + 1 * direction)
                {

                    if (i >= 0 && i < GridManager.instance.GridSize)
                    {
                        if (direction > 0)
                            connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                        else
                            connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                    }

                    if (i < 0 || i > GridManager.instance.GridSize - 1 || (int)dot.SubGridPosition.x == i)
                        continue;

                    if (!dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).IsPaintable)
                    {
                        Debug.LogWarning("No valid connection. Path isn't clear!");
                        return false;
                    }

                }
            }
        }
        return true;
    }

    private bool IsPathClearHorizontal(Dot baseDot, Dot dot, Connection connection)
    {
        Debug.Log("hoizontal");
        var direction = Grid.FromSideToSideDirectionHorizontal(baseDot.ParentSubGrid.ParentGrid.CubeSide,
            dot.ParentSubGrid.ParentGrid.CubeSide);
        var end = (direction > 0) ? (int)GridManager.instance.GridSize : -1;

        if (dot.ParentSubGrid.ParentGrid.CubeSide == baseDot.ParentSubGrid.ParentGrid.CubeSide)
        {
            var loopOperator = (dot.SubGridPosition.x > baseDot.SubGridPosition.x) ? 1 : -1;

            if (loopOperator > 0)
                connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineHorzPosGo, baseDot.ParentSubGrid);
            else
                connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineHorzNegGo, baseDot.ParentSubGrid);

            for (int i = (int)baseDot.SubGridPosition.x + 1 * loopOperator; i != (int)dot.SubGridPosition.x; i = i + 1 * loopOperator)
            {
                if (i >= 0 && i < GridManager.instance.GridSize)
                {
                    if (loopOperator > 0)
                        connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                    else
                        connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                }

                if (i < 0 || i >= GridManager.instance.GridSize - 1)
                    continue;
                if (!baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).IsPaintable)
                {
                    Debug.LogWarning("No valid connection. Path isn't clear!");
                    return false;
                }
            }
        }
        else
        {
            Debug.Log("woho");

            var startpar = (int)baseDot.SubGridPosition.x + 1 * direction;
            var directionPar = direction;
            var endPar = end;

            if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back &&
                 dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left))
            {
                directionPar *= -1;
                startpar = (int)baseDot.SubGridPosition.x;
                endPar = Mathf.Abs(end - GridManager.instance.GridSize);
            }
            else if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back &&
                 dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right))
            {
                directionPar *= -1;
                startpar = (int)baseDot.SubGridPosition.x;
                endPar = Mathf.Abs(end - GridManager.instance.GridSize);
            }

            if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back) &&
                (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left ||
                 dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right))
            {
                if (direction > 0)
                    connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineHorzNegGo, baseDot.ParentSubGrid);
                else
                    connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineHorzPosGo, baseDot.ParentSubGrid);
            }
            else
            {
                if (direction > 0)
                    connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineHorzPosGo, baseDot.ParentSubGrid);
                else
                    connection.AddPartToConnection(baseDot.ParentSubGrid.ConnectionLines.connectionLineHorzNegGo, baseDot.ParentSubGrid);
            }


            if ((int)baseDot.SubGridPosition.x + 1 * direction == end && (end >= 0 && end < GridManager.instance.GridSize))
            {
                if (direction > 0)
                    connection.AddPartToConnection(
                        baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(end, (int)baseDot.SubGridPosition.y)
                            .ConnectionLines.connectionLineVertPosGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(end, (int)baseDot.SubGridPosition.y));
                else
                    connection.AddPartToConnection(
                        baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(end, (int)baseDot.SubGridPosition.y)
                            .ConnectionLines.connectionLineHorzNegGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(end, (int)baseDot.SubGridPosition.y));
            }

            Debug.Log("dsg: " + startpar + " " + endPar + " " + directionPar);

            if (!(direction > 0 && baseDot.SubGridPosition.x >= GridManager.instance.GridSize - 1) && !(direction < 0 && (int)baseDot.SubGridPosition.x == 0))
            {
                Debug.Log((int)baseDot.SubGridPosition.x + 1 * direction + "  " + end + "  " + direction);
                Debug.Log("dsg: " + startpar + " " + endPar + " " + directionPar);

                for (int i = startpar; i != endPar; i = i + 1 * directionPar)
                {
                    if (i < 0 || i > GridManager.instance.GridSize - 1 || i == (int)baseDot.ParentSubGrid.SubGridPosition.x)
                        continue;

                    if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.bottom || baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top) && (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left))
                    {
                        Debug.Log("njgsnj");
                        Debug.Log(i + " " + ((int)dot.SubGridPosition.x));
                        if (!baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.x).IsPaintable)
                        {
                            Debug.LogWarning("No valid connection. Path isn't clear!");
                            return false;
                        }

                        if (i >= 0 && i < GridManager.instance.GridSize)
                        {
                            if (direction > 0)
                                connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.x).ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.x));
                            else
                                connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.x).ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.x));
                        }
                    }
                    else
                    {
                        if (!baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).IsPaintable)
                        {
                            Debug.LogWarning("No valid connection. Path isn't clear!");
                            return false;
                        }

                        if (!((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back) && (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left || dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right)) && (i >= 0 && i < GridManager.instance.GridSize))
                        {
                            Debug.Log("dsgdsrerewrew");
                            if (direction > 0)
                                connection.AddPartToConnection(baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzPosGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                            else
                                connection.AddPartToConnection(baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzNegGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                        }
                        else if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back) && (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left || dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right))
                        {
                            if (direction > 0)
                                connection.AddPartToConnection(baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzNegGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                            else
                                connection.AddPartToConnection(baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzPosGo, baseDot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                        }
                    }
                }

            }

            Debug.Log("yiha");

            if (CubeLookup.GetDirectionForSecondSideHorz(baseDot.ParentSubGrid.ParentGrid.CubeSide) ==
                ConnectionDirectionGeneral.horizontal)
            {
                Debug.Log("hoz noraml");
                Debug.Log((Mathf.Abs(end - GridManager.instance.GridSize)) + " " + dot.SubGridPosition.x + " " + direction);

                var start = Mathf.Abs(end - GridManager.instance.GridSize);

                if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back) &&
                    (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left ||
                     dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right))
                {
                    if (direction > 0)
                        connection.AddPartToConnection(
                            dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - 1, (int)dot.SubGridPosition.y)
                                .ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - 1, (int)dot.SubGridPosition.y));
                    else
                        connection.AddPartToConnection(
                            dot.ParentSubGrid.ParentGrid.GetSubGridAt(0, (int)dot.SubGridPosition.y)
                                .ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(0, (int)dot.SubGridPosition.y));
                }
                else
                {
                    if (direction > 0)
                        connection.AddPartToConnection(
                            dot.ParentSubGrid.ParentGrid.GetSubGridAt(0, (int)dot.SubGridPosition.y)
                                .ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(0, (int)dot.SubGridPosition.y));
                    else
                        connection.AddPartToConnection(
                            dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - 1, (int)dot.SubGridPosition.y)
                                .ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - 1, (int)dot.SubGridPosition.y));
                }



                if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back &&
                     dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.right))
                {
                    start = GridManager.instance.GridSize;
                    direction *= -1;
                }

                for (int i = start;
                    i != (int)dot.SubGridPosition.x;
                    i = i + 1 * direction)
                {
                    if (i >= 0 && i < GridManager.instance.GridSize)
                    {
                        if ((baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.back) &&
                            (dot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.left))
                        {
                            if (direction > 0)
                                connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - i, (int)dot.SubGridPosition.y));
                            else
                                connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(GridManager.instance.GridSize - i, (int)dot.SubGridPosition.y));
                        }
                        else
                        {
                            if (direction > 0)
                                connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                            else
                                connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).ConnectionLines.connectionLineHorzNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y));
                        }



                    }

                    if (i < 0 || i > GridManager.instance.GridSize - 1)
                        continue;

                    if (!dot.ParentSubGrid.ParentGrid.GetSubGridAt(i, (int)dot.SubGridPosition.y).IsPaintable)
                    {
                        Debug.LogWarning("No valid connection. Path isn't clear!");
                        return false;
                    }


                }
            }
            else
            {
                Debug.Log("hoz yeah");
                var start = 0;
                if (baseDot.ParentSubGrid.ParentGrid.CubeSide == CubeSide.top)
                    start = GridManager.instance.GridSize;

                if (start > 0) direction = -1;
                else direction = 1;


                if (direction > 0)
                    connection.AddPartToConnection(
                        dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, 0)
                            .ConnectionLines.connectionLineVetNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, 0));
                else
                    connection.AddPartToConnection(
                        dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, GridManager.instance.GridSize - 1)
                            .ConnectionLines.connectionLineVertPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, GridManager.instance.GridSize - 1));

                for (int i = start;
                    i != (int)dot.SubGridPosition.y;
                    i = i + 1 * direction)
                {
                    if (i < 0 || i > GridManager.instance.GridSize - 1)
                        continue;

                    if (!dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i).IsPaintable)
                    {
                        Debug.LogWarning("No valid connection. Path isn't clear!");
                        return false;
                    }

                    if (i >= 0 && i < GridManager.instance.GridSize)
                    {
                        if (direction > 0)
                            connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i).ConnectionLines.connectionLineVertPosGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                        else
                            connection.AddPartToConnection(dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i).ConnectionLines.connectionLineVetNegGo, dot.ParentSubGrid.ParentGrid.GetSubGridAt((int)dot.SubGridPosition.x, i));
                    }
                }
            }
        }
        return true;
    }
}
