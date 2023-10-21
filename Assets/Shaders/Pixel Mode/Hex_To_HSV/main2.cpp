#include <iostream>
#include <string>
#include <vector>
#include <sstream>

int main()
{

    std::vector<std::string> rgb_values;
    std::string line;

    while (std::getline(std::cin, line))
    {
        rgb_values.push_back(line);
    }

    for (std::string s : rgb_values)
    {
        std::stringstream ss(s);
        unsigned val;
        int r_i, g_i, b_i;

        ss >> r_i >> g_i >> b_i;

        float r = r_i / 255.0;
        float g = g_i / 255.0;
        float b = b_i / 255.0;

        std::cout << "float3(" << r << ", " << g << ", " << b << ")," << "\n";
    }

}
