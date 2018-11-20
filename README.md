# Bubble Economy

The idea here is to create a simulation where there are multiple cities and each city has it's own Economy.<br>
The cities will give it's excess Economy to it's neighbours when it is doing really well.<br>

I'm interested in seeing the results of this sim.
I think it could go a couple different ways:
- Each city is going to do 'well' after the other, city 1# is going to do well first and after that city #2, etc.
- Only a specific 'group' is going to do well because they keep 'trading' excess with eachother.
- ...

## City
A city contains a couple things
- non-working population (amount)
- working population (amount)
- economy (amount)

### Non-working population
The non-working population describes how many people inside the city are not working.<br>
The **higher** the non-working population, the faster the Economy **decreases**.<br>
The **lower** the non-working population, the faster the Economy **increases**.<br>

### Working population
The working population describes how many people inside the city are working.<br>
The **higher** the working population, the faster the Economy **increases**.<br>
The **lower** the working population, the faster the Economy **decreases**.<br>
The working population has a limit, if this limit is reached all the 'new' population will be moved automatically to the non-working population.<br>

### Economy
The Economy describes how 'well' a city is doing.<br>
It can be influenced by the amount of working- / non-working population.<br>
If the Economy is doing well, the City will attract populations from other Cities.<br>
These new populations will be first added to the non-working population and have a chance to be added to the working population if there is room.<br>
Also, if the Economy is doing 'too well', it will 'give' it's excess 'economy' to it's neighbouring cities.<br>

## Version 1
[demo version 1](https://i.imgur.com/iNCG6Sv.gifv)<br>
What you see in version 1 is nothing special..<br>
Cities do well with a high amount of workers (as expected)<br>
But the non-workers don't really affect the economy that much..<br>
Once you increase the neighbour range, you'll see that everyone shares and all do good (but the economies are not exactly the same)<br>
### Features
Every tick<br>
- `economy -= nonWorkingPopulation * 0.01f;`<br>
- `economy += workingPopulation * 0.1f;`<br>
- `economy = economy > 0 ? economy - 0.1f : 0;`<br>
- Chance that a working/non-working will leave: **20%**<br>
  They will move to the City with the heighest Economy<br>
- Chance that a non-working will work: **40%**<br>

red: amount of nonworking<br>
green: amount of economy<br>
blue: range in which this city can find neighbours<br>
