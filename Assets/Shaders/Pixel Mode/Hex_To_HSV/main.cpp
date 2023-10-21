#include <iostream>
#include <string>
#include <vector>
#include <sstream>

int main()
{

    std::vector<std::string> hex_values;
    std::string line;

    while (std::getline(std::cin, line))
    {
        hex_values.push_back(line);
    }

    for (std::string s : hex_values)
    {
        std::stringstream ss;
        unsigned val;

        ss << std::hex << s;
        ss >> val;

        //std::cout << ((val & 0xFF0000) >> 16) << " " << ((val & 0xFF00) >> 8) << " " << (val & 0xFF) << "\n";

        float r = ((val & 0xFF0000) >> 16) / 255.0f;
        float g = ((val & 0xFF00) >> 8) / 255.0f;
        float b = ((val & 0xFF)) / 255.0f;

        //std::cout << "{" << r << "," << g << "," << b << "}," << "\n";
        std::cout << "float3(" << r << ", " << g << ", " << b << ")," << "\n";
    }

}
