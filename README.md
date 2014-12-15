dyndnsservice
=============

A simple Dynamic DNS system. I wrote this to work with my [Mikrotik][1] router to update DNS records for my WAN links. More details on that later, but here is the basic script used:

  :local ddnshost "wan1" # what you want to update on the host, must be unique
  :local key "key"
  :local updatehost "192.168.0.80" # IP of machine running the dyndns service
  :local WANinterface "WAN1" # interface to get IP of
  :local outputfile ("wan1" . ".txt") # text result file
  
  # Internal processing below...
  # ----------------------------------
  :local ipv4addr
  
  # Get WAN interface IP address
  :set ipv4addr [/ip address get [/ip address find interface=$WANinterface] address]
  :set ipv4addr [:pick [:tostr $ipv4addr] 0 [:find [:tostr $ipv4addr] "/"]]
  
  :if ([:len $ipv4addr] = 0) do={
    :log error ("DYNDNS: Could not get IP for interface " . $WANinterface)
    :error ("DYNDNS: Could not get IP for interface " . $WANinterface)
  }
  
  :log info ("DYNDNS: Updating DDNS IPv4 address" . " Client IPv4 address to new IP " . $ipv4addr . "...")
  
  /tool fetch url="http://$updatehost/nic/update?hostname=$ddnshost&myip=$ipv4addr"  \
  dst-path=$outputfile
                                
  :log info ("DYNDNS:  " .$ddnshost . " " . [/file get ($outputfile) contents])
  /file remove ($outputfile)
  
