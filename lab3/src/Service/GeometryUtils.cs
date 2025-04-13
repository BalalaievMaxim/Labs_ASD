namespace lab3;

public static class GeometryUtils
{
    public static (double, double) FindIntersection(
        double cx, double cy, double radius,
        double x1, double y1, double x2, double y2)
    {
        double dx = x2 - x1;
        double dy = y2 - y1;

        // Calculate the numerator for the projection parameter t
        double numerator = (cx - x1) * dx + (cy - y1) * dy;
        // Calculate the denominator which is the squared length of the line direction vector
        double denominator = dx * dx + dy * dy;

        // Compute the parameter t for the projection
        double t = numerator / denominator;

        // Calculate the intersection point coordinates
        double intersectionX = x1 + t * dx;
        double intersectionY = y1 + t * dy;

        return (intersectionX, intersectionY);
    }
}